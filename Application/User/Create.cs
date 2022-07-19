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
                var modelErrors = new ModelErrorResult<Unit>();
                Employee imtsUser = null;
                //Create 
                if (request.mode == FormViewMode.Create)
                {
                    //If Imts user
                    if(!(await validateImtsUser(request.appUserDTO, modelErrors))) return modelErrors;
                    //is an existed user?
                    var appUser = await _userManager.FindByEmailAsync(request.appUserDTO.Email);
                    IdentityResult result;
                    if (appUser == null)
                    {
                        if(string.IsNullOrWhiteSpace(request.appUserDTO.Password))
                            return Result<Unit>.Failure("Password required");
                        appUser = new AppUser();
                        CreateAppUserModel(appUser, request.appUserDTO);
                        result = await _userManager.CreateAsync(appUser, request.appUserDTO.Password);
                        if (result.Succeeded)
                        {
                            await _unitOfWork.Users.addRoleToUser(appUser, _userAccessor.GetOfficeId(), request.appUserDTO.RoleName);
                            var succeeded = await _unitOfWork.TryCommit();
                            if (succeeded)
                                return Result<Unit>.Success(Unit.Value);
                            else
                                return Result<Unit>.Failure("Failed to create the user in the office");

                        }
                        else
                        {
                            return Result<Unit>.Failure("Failed to create the user");
                        }

                    }
                    else
                    {
                        CreateAppUserModel(appUser, request.appUserDTO);
                        result = await _userManager.UpdateAsync(appUser);
                        if (result.Succeeded)
                        {


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
                        
                        //If Imts user
                        if(!(await validateImtsUser(request.appUserDTO, modelErrors))) return modelErrors;

                        CreateAppUserModel(appUser, request.appUserDTO);

                        IdentityResult result;
                        result = await _userManager.UpdateAsync(appUser);
                        if (result.Succeeded)
                        {
                            var appUserOfficerole = await _unitOfWork.Users.getAppUserOfficeRoleByUserAndOffice(appUser.Id, _userAccessor.GetOfficeId());
                            if (appUserOfficerole.Role.RoleName != request.appUserDTO.RoleName)
                            {
                                await _unitOfWork.Users.removeAppUserOfficeRole(appUser.Id, _userAccessor.GetOfficeId());
                                await _unitOfWork.Users.addRoleToUser(appUser, _userAccessor.GetOfficeId(), request.appUserDTO.RoleName);
                                var succeeded = await _unitOfWork.TryCommit();
                                if (!succeeded)
                                    return Result<Unit>.Failure("Failed to update the user role");
                            }
                        }
                        else
                        {
                            return Result<Unit>.Failure("Failed to update the user");
                        }
                    }
                    else
                    {
                        return Result<Unit>.Failure("User Not FOund");
                    }
                    return Result<Unit>.Success(Unit.Value);
                }
            }
            private async Task<bool> validateImtsUser(AppUserDTO appUserDTO, ModelErrorResult<Unit> modelErrors)
            {
                var valid = true;

                if (appUserDTO.IsImtsUser)
                {
                    if (!String.IsNullOrWhiteSpace(appUserDTO.ImtsUserName))
                    {
                        var imtsUser = await findImtsUser(appUserDTO.ImtsUserName);
                        if (imtsUser != null)
                        {
                            appUserDTO.ImtsEmployeeId = imtsUser.id;
                        }
                        else
                        {
                            modelErrors.ModelErrors.Add("IsImtsUserName ", appUserDTO.ImtsUserName + " Not valid IMTS username.");
                            valid = false;
                        }
                    }else
                    {
                        valid = false;
                        modelErrors.ModelErrors.Add("IsImtsUserName ", appUserDTO.ImtsUserName + " Not valid IMTS username.");
                    }
                }else{
                    appUserDTO.ImtsUserName = null;
                    appUserDTO.ImtsEmployeeId = null;
                }
                return valid;
            }
            private void CreateAppUserModel(AppUser user, AppUserDTO appUserDTO)
            {
                if(string.IsNullOrWhiteSpace(appUserDTO.Id))
                {
                    user = new AppUser();
                    user.CreateDate = DateTime.Now;
                }
                user.Email = appUserDTO.Email.ToLower();
                user.UserName = appUserDTO.Email.ToLower();
                user.FirstName = appUserDTO.FirstName;
                user.LastName = appUserDTO.LastName;
                user.MainOfficeId = user.MainOfficeId == 0 ? _userAccessor.GetOfficeId() : user.MainOfficeId;
                user.IsImtsUser = appUserDTO.IsImtsUser;
                user.UpdatedDate = DateTime.Now;
                user.ImtsUserName = appUserDTO.ImtsUserName;
                user.ImtsEmployeeId = appUserDTO.ImtsEmployeeId;
            }
            
            private async Task<Employee> findImtsUser(string userName)
            {
                return await _imtsUserService.GetImtsUserByUserName(userName);
            }
        }
    }
}