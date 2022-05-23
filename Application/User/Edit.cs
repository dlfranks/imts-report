using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Imts;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Domain.imts;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.User
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public AppUserDTO appUserDTO { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IUserAccessor _userAccessor;
            private readonly IUnitOfWork _unitOfWork;
            private readonly UserManager<AppUser> _userManager;
            private readonly ImtsUserService _imtsUserService;
            private readonly IMapper _mapper;
            public Handler(IUserAccessor userAccessor, IUnitOfWork unitOfWork, ImtsUserService imtsUserService, UserManager<AppUser> userManager, IMapper mapper)
            {
                _mapper = mapper;
                _imtsUserService = imtsUserService;
                _userManager = userManager;
                _unitOfWork = unitOfWork;
                _userAccessor = userAccessor;

            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                Employee imtsUser = null;
                

                AppUser user = await _userManager.Users.Where(q => q.Id == request.appUserDTO.Id).FirstOrDefaultAsync();
                if(user == null) return Result<Unit>.Failure("User Not Found");

                user.UserName = request.appUserDTO.Email;
                user.FirstName = request.appUserDTO.FirstName;
                user.LastName = request.appUserDTO.LastName;
                user.Email = request.appUserDTO.Email;
                user.IsImtsUser = request.appUserDTO.IsImtsUser;
                user.ImtsUserName = request.appUserDTO.ImtsUserName;
                user.ImtsEmployeeId = request.appUserDTO.ImtsEmployeeId;
                user.UpdatedDate = DateTime.Now;
                
                //Role check
                var userOfficeRole = await _unitOfWork.Users.getAppUsersOfficeRolesByUserAndOffice(user.Id, _userAccessor.GetOfficeId());
                var role = await _unitOfWork.OfficeRoles.get().Where(q => q.RoleName == request.appUserDTO.RoleName.ToLower()).FirstAsync();
                userOfficeRole.RoleId = role.Id;
                userOfficeRole.Role = role;
                userOfficeRole.AppUser = user;
                _unitOfWork.Users.updateAppUserOfficeRole(userOfficeRole);
                //ImtsUser
                if (request.appUserDTO.IsImtsUser)
                {
                    if(string.IsNullOrWhiteSpace(request.appUserDTO.ImtsUserName))
                        return Result<Unit>.Failure(request.appUserDTO.ImtsUserName + "Fill your username");

                    imtsUser = await _imtsUserService.GetImtsUserByUserName(request.appUserDTO.ImtsUserName);

                    if (imtsUser == null) return Result<Unit>.Failure(request.appUserDTO.ImtsUserName + "UserName Not Found From IMTS.");
                    
                    user.MainOfficeId = imtsUser.mainOfficeId;
                    user.ImtsEmployeeId = imtsUser.id;

                }else{
                    user.ImtsUserName = null;
                    user.ImtsEmployeeId = null;
                }
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    return Result<Unit>.Failure(request.appUserDTO.ImtsUserName + "Failed to update the user");

                
                if (await _unitOfWork.TryCommit())
                {
                    var appUserDto = _mapper.Map<AppUserDTO>(user);
                    return Result<Unit>.Success(Unit.Value);
                }
                else
                {
                    return Result<Unit>.Failure("Failed to update the user's role.");
                }


            }

            
            
        }
    }
}