using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Application.Imts;
using Application.Interfaces;
using Domain;
using Domain.imts;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.User
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public FormViewMode mode { get; set; }
            public AppUserDTO appUserDTO { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(q => q.appUserDTO).SetValidator(new AppuserValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IUserAccessor _userAccessor;
            private readonly IUnitOfWork _unitOfWork;
            private readonly UserManager<AppUser> _userManager;
            private readonly ImtsUserService _imtsUserService;

            public Handler(IUserAccessor userAccessor, UserManager<AppUser> userManager, IUnitOfWork unitOfWork, ImtsUserService imtsUserService)
            {
                _imtsUserService = imtsUserService;

                _userManager = userManager;
                _unitOfWork = unitOfWork;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                Employee imtsUser = null; bool isCreate = false;
                AppUser user = await _userManager.Users.Where(q => q.Id == request.appUserDTO.Id).FirstOrDefaultAsync();
                if(user == null){
                    //create a Appuser
                    isCreate = true;
                    user = new AppUser();
                    user.CreateDate = DateTime.Now;
                    user.MainOfficeId = _userAccessor.GetOfficeId();
                    user.Email = request.appUserDTO.Email;
                    user.UserName = request.appUserDTO.Email;
                }
                user.FirstName = request.appUserDTO.FirstName;
                user.LastName = request.appUserDTO.LastName;
                user.IsImtsUser = request.appUserDTO.IsImtsUser;
                user.UpdatedDate = DateTime.Now;
                //If Imts user
                if (request.appUserDTO.IsImtsUser)
                {
                    if (String.IsNullOrWhiteSpace(request.appUserDTO.ImtsUserName))
                        return Result<Unit>.Failure(request.appUserDTO.ImtsUserName + "Fill UserName");
                    imtsUser = await _imtsUserService.GetImtsUserByUserName(request.appUserDTO.ImtsUserName);
                    if (imtsUser == null) return Result<Unit>.Failure(request.appUserDTO.ImtsUserName + " Not Found");
                    user.MainOfficeId = imtsUser.mainOfficeId;
                    user.ImtsEmployeeId = imtsUser.id;

                }
                IdentityResult result;
                if(isCreate)
                {
                    result = await _userManager.CreateAsync(user, request.appUserDTO.Password);
                }else{
                    result = await _userManager.UpdateAsync(user);
                }
                 
                //create or Edit AppUserOfficeRole
                
                if (result.Succeeded){
                    if(FormViewMode.Create == request.mode)
                    {
                        await _unitOfWork.Users.addRoleToUser(user.Id, _userAccessor.GetOfficeId(), request.appUserDTO.RoleName);
                        
                    }else
                    {
                        var userOfficeRole = await _unitOfWork.Users.getAppUserOfficeRoleByUserAndOffice(user.Id, _userAccessor.GetOfficeId());
                        if(userOfficeRole == null) return Result<Unit>.Failure("Failed to load a user's role.");
                            if(userOfficeRole.Role.RoleName != request.appUserDTO.RoleName){
                                await _unitOfWork.Users.removeAppUserOfficeRole(user.Id, _userAccessor.GetOfficeId());
                                //await _unitOfWork.Commit();
                                await _unitOfWork.Users.addRoleToUser(user.Id, _userAccessor.GetOfficeId(), request.appUserDTO.RoleName);
                            }
                        }
                }else
                    return Result<Unit>.Failure("Failed to create a user.");
                //
                if (await _unitOfWork.TryCommit())
                {
                    return Result<Unit>.Success(Unit.Value);
                }
                else
                {
                    return Result<Unit>.Failure("Failed to create a user's role.");
                }
            }
        }
    }
}