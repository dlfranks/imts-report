using System;
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
            private readonly UserManager<AppUser> _userManager;
            private readonly ImtsUserService _imtsUserService;
            private readonly Persistence.AppContext _appContext;

            public Handler(IUserAccessor userAccessor, Persistence.AppContext appContext, ImtsUserService imtsUserService)
            {
                _appContext = appContext;
                _imtsUserService = imtsUserService;
                _userAccessor = userAccessor;

            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                Employee imtsUser = null;
                AppUser user = new AppUser();
                user.UserName = request.appUserDTO.Email;
                user.FirstName = request.appUserDTO.FirstName;
                user.LastName = request.appUserDTO.LastName;
                user.Email = request.appUserDTO.Email;
                user.MainOfficeId = _userAccessor.GetOfficeId();
                user.IsImtsUser = request.appUserDTO.IsImtsUser;
                user.CreateDate = DateTime.Now;
                user.UpdatedDate = DateTime.Now;
                if (request.appUserDTO.IsImtsUser)
                {
                    if (String.IsNullOrWhiteSpace(request.appUserDTO.ImtsEmployeeUserName))
                        return Result<Unit>.Failure(request.appUserDTO.ImtsEmployeeUserName + "Fill UserName");
                    imtsUser = await _imtsUserService.GetImtsUserByUserName(request.appUserDTO.ImtsEmployeeUserName);
                    if (imtsUser == null) return Result<Unit>.Failure(request.appUserDTO.ImtsEmployeeUserName + " Not Found");
                    user.MainOfficeId = imtsUser.mainOfficeId;
                    user.ImtsEmployeeId = imtsUser.id;

                }
                var result = await _userManager.CreateAsync(user, request.appUserDTO.Password);


                if (result.Succeeded)
                    return Result<Unit>.Success(Unit.Value);
                    //await _unitOfWork.AppUsers.addRoleToUser(user, _userAccessor.GetOfficeId(), request.appUserDTO.RoleName);
                else
                    return Result<Unit>.Failure("Failed to create a user.");

                //await _unitOfWork.TryCommit()
                if (true)
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