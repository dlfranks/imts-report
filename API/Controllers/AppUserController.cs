using System;
using System.Threading.Tasks;
using API.DTOs;
using API.Services;
using Domain;
using Domain.imts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AppUserController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        public AppUserController(
            UserManager<AppUser> userManager, 
            UserService userService,
            IEntityScope scope
            ) : base(userService, scope)
                
        {
            _userManager = userManager;

        }

        [HttpPost]
        public async Task<ActionResult<string>> Create(RegisterDTO registerDto)
        {
            if(!await (_userService.permissionForEmployee(PermissionAction.Create))) return Unauthorized();

            //validate an email
            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                ModelState.AddModelError("email", "Email taken");
                return ValidationProblem();
            }
            
            try
            {
                var appUserResult = await _userService.CreateUser(registerDto);
                if(appUserResult.IsSuccess)
                {
                    return Ok(appUserResult.Value.Id);
                }

                
            }catch(Exception e)
            {
                var msg = "Internal error, unable to create a user" + e;
                //Elmah
            }
            return BadRequest("Problem registering user");
        }

        
    }
}