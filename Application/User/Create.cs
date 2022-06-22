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
                Employee imtsUser = null;
                //Create
                if (request.mode == FormViewMode.Create)
                {
                    //If Imts user
                    if (request.appUserDTO.IsImtsUser)
                    {
                        if (!String.IsNullOrWhiteSpace(request.appUserDTO.ImtsUserName))
                        {
                            imtsUser = await findImtsUser(request.appUserDTO.ImtsUserName);
                            if (imtsUser != null)
                            {
                                request.appUserDTO.MainOfficeId = imtsUser.mainOfficeId;
                                request.appUserDTO.ImtsEmployeeId = imtsUser.id;
                            }
                            else
                            {
                                return Result<Unit>.Failure(request.appUserDTO.ImtsUserName + " failed to find the IMTS user");
                            }
                        }

                    }
                    //is an existed user?
                    var appUser = await _userManager.FindByEmailAsync(request.appUserDTO.Email);
                    IdentityResult result;
                    if (appUser == null)
                    {
                        result = await createAppUser(request.appUserDTO);
                        if (result.Succeeded)
                        {
                            //var appUserOfficeroleResult = await createAppUserOfficeRole(appUser.Id, _userAccessor.GetOfficeId(), request.appUserDTO.RoleName);
                            await _unitOfWork.Users.addRoleToUser(appUser, _userAccessor.GetOfficeId(), request.appUserDTO.RoleName);
                            if (await _unitOfWork.TryCommit())
                                return Result<Unit>.Success(Unit.Value);
                            else return Result<Unit>.Failure("Failed to create the user in the office");

                        }
                        else
                        {
                            return Result<Unit>.Failure("Failed to create the user");
                        }

                    }
                    else
                    {
                        updateAppUser(appUser, request.appUserDTO);
                        result = await _userManager.UpdateAsync(appUser);
                        if (result.Succeeded)
                        {

                            //var appUserOfficeroleResult = await createAppUserOfficeRole(appUser.Id, _userAccessor.GetOfficeId(), request.appUserDTO.RoleName);
                            await _unitOfWork.Users.addRoleToUser(appUser, _userAccessor.GetOfficeId(), request.appUserDTO.RoleName);
                            if (await _unitOfWork.TryCommit())
                                return Result<Unit>.Success(Unit.Value);
                            else return Result<Unit>.Failure("Failed to create the user in the office");

                        }
                        else
                        {
                            return Result<Unit>.Failure("Failed to create the user");
                        }

                    }
                    //Elmah(result.Errors)


                }//Edit
                else
                {
                    AppUser appUser = await _userManager.Users.Where(q => q.Id == request.appUserDTO.Id).FirstOrDefaultAsync();
                    if (appUser != null)
                    {
                        updateAppUser(appUser, request.appUserDTO);
                        //If Imts user
                        if (appUser.IsImtsUser)
                        {
                            if (!String.IsNullOrWhiteSpace(appUser.ImtsUserName))
                            {
                                imtsUser = await findImtsUser(appUser.ImtsUserName);
                                if (imtsUser != null)
                                {
                                    appUser.ImtsEmployeeId = imtsUser.id;
                                }
                                else
                                {
                                    return Result<Unit>.Failure(request.appUserDTO.ImtsUserName + " failed to find the IMTS user");
                                }
                            }

                        }
                        IdentityResult result;
                        result = await _userManager.UpdateAsync(appUser);
                        if (result.Succeeded)
                        {
                            var appUserOfficerole = await _unitOfWork.Users.getAppUserOfficeRoleByUserAndOffice(appUser.Id, _userAccessor.GetOfficeId());
                            if(appUserOfficerole.Role.RoleName != request.appUserDTO.RoleName)
                            {
                                var appUserOfficeroleResult = await createAppUserOfficeRole(appUser, _userAccessor.GetOfficeId(), request.appUserDTO.RoleName);
                            }
                            
                            return Result<Unit>.Success(Unit.Value);
                        }
                        else
                        {
                            return Result<Unit>.Failure("Failed to update the user");
                        }


                    }
                    return Result<Unit>.Failure("User Not FOund");
                }
            }
            private void updateAppUser(AppUser user, AppUserDTO appUserDTO)
            {
                user.Email = appUserDTO.Email;
                user.UserName = appUserDTO.Email;
                user.FirstName = appUserDTO.FirstName;
                user.LastName = appUserDTO.LastName;
                user.MainOfficeId = user.MainOfficeId == 0 ? _userAccessor.GetOfficeId() : user.MainOfficeId;
                user.IsImtsUser = appUserDTO.IsImtsUser;
                user.UpdatedDate = DateTime.Now;
                user.ImtsUserName = appUserDTO.IsImtsUser ? appUserDTO.ImtsUserName : null;
                user.ImtsEmployeeId = appUserDTO.IsImtsUser ? appUserDTO.ImtsEmployeeId : null;
            }
            private async Task<IdentityResult> createAppUser(AppUserDTO appUserDTO)
            {
                var user = new AppUser();
                user.CreateDate = DateTime.Now;
                user.MainOfficeId = _userAccessor.GetOfficeId();
                user.Email = appUserDTO.Email;
                user.UserName = appUserDTO.Email;
                user.FirstName = appUserDTO.FirstName;
                user.LastName = appUserDTO.LastName;
                user.IsImtsUser = appUserDTO.IsImtsUser;
                user.UpdatedDate = DateTime.Now;
                user.ImtsUserName = appUserDTO.IsImtsUser ? appUserDTO.ImtsUserName : null;
                user.ImtsEmployeeId = appUserDTO.IsImtsUser ? appUserDTO.ImtsEmployeeId : null;

                return await _userManager.CreateAsync(user, appUserDTO.Password);
            }
            private async Task<bool> createAppUserOfficeRole(AppUser appUser, int officeId, string roleName)
            {
                await _unitOfWork.Users.removeAppUserOfficeRole(appUser.Id, _userAccessor.GetOfficeId());
                await _unitOfWork.Users.addRoleToUser(appUser, officeId, roleName);
                
                return await _unitOfWork.TryCommit();

            }
            private async Task<Employee> findImtsUser(string userName)
            {
                return await _imtsUserService.GetImtsUserByUserName(userName);
            }
        }
    }
}