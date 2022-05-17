using System;
using System.Threading.Tasks;
using API.DTOs;
using API.Middleware;
using API.Services;
using API.Services.Interfaces;
using API.ViewModels;
using Domain;
using Domain.imts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class AppUserController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        public AppUserController(
            UserManager<AppUser> userManager,
            UserService userService
            ) : base(userService)

        {
            _userManager = userManager;
        }

        [HttpGet("lookupusername")]
        public async Task<IActionResult> LookupUsernameForCreate(string userName)
        {
            if (!(await _userService.permissionForEmployee(PermissionAction.Create, null, autoThrow: false)))
                return Unauthorized();

            var appUser = await _userManager.FindByNameAsync(userName);

            if (appUser != null) return Ok(true);
            return Ok(false);
        }
        [HttpPost]
        public async Task<IActionResult> Create(RegisterDTO registerDto)
        {
            return await AddUpdateAppUser(registerDto, FormViewMode.Create);
        }
        protected async Task<IActionResult> AddUpdateAppUser(RegisterDTO registerDto, FormViewMode mode)
        {

            var currentUserSettings = await _userService.CreateUserSettings();

            if (!await (_userService.permissionForEmployee(PermissionAction.Create))) return Unauthorized();

            //validate an username
            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Email))
            {
                ModelState.AddModelError("email", "Email taken");
                return ValidationProblem();
            }
            if (registerDto.IsImtsUser)
            {
                if (!(await _userService.IsImtsUser(registerDto.ImtsUserName)))
                {
                    ModelState.AddModelError("IsImtsUser", "Not Found");
                    return ValidationProblem();
                }
            }

            try
            {

                var appUserResult = await _userService.CreateUser(registerDto, currentUserSettings.currentOfficeId);
                if (appUserResult.IsSuccess)
                {
                    return Ok(appUserResult.Value.Id);
                }
            }
            catch (Exception e)
            {
                var msg = "Internal error, unable to create a user" + e;
                //Elmah
            }
            return BadRequest("Problem registering user");
        }


    }
}