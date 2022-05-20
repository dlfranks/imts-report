using System;
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
using Persistence;

namespace Application.User
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
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
                AppUser user = new AppUser
                {
                    UserName = request.appUserDTO.Email,
                    FirstName = request.appUserDTO.FirstName,
                    LastName = request.appUserDTO.LastName,
                    Email = request.appUserDTO.Email,
                    MainOfficeId = _userAccessor.GetOfficeId(),
                    IsImtsUser = request.appUserDTO.IsImtsUser,
                    CreateDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };
                if (request.appUserDTO.IsImtsUser)
                {
                    if(String.IsNullOrWhiteSpace(request.appUserDTO.ImtsUserName))
                        return Result<Unit>.Failure(request.appUserDTO.ImtsUserName + "Fill UserName");
                    imtsUser = await _imtsUserService.GetImtsUserByUserName(request.appUserDTO.ImtsUserName);
                    if (imtsUser == null) return Result<Unit>.Failure(request.appUserDTO.ImtsUserName + " Not Found");
                    user.MainOfficeId = imtsUser.mainOfficeId;
                    user.ImtsEmployeeId = imtsUser.id;

                }
                var result = await _userManager.CreateAsync(user, request.appUserDTO.Password);
                
                
                if (result.Succeeded)
                    await _unitOfWork.Users.addRoleToUser(user, _userAccessor.GetOfficeId(), request.appUserDTO.RoleName);
                else
                    return Result<Unit>.Failure("Failed to create a user.");

                
                if (await _unitOfWork.TryCommit())
                {
                    var appUserDto = _mapper.Map<AppUserDTO>(user);
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