using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs;
using API.Services;
using Application.Core;
using Domain;
using Domain.imts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;


        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManger,
            TokenService tokenService,
            UserService userService,
            IEntityScope scope
            ) : base(userService, scope)
        {
            _signInManager = signInManger;
            _userManager = userManager;
        }
        [HttpPost("switchoffice")]
        public async Task<ActionResult<UserDTO>> SwitchOffice(SwitchOfficeDTO model)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var user = await _userManager.FindByNameAsync(currentUser.UserName);
            _userService.Scope.officeId = model.newOfficeId;
            return CreateUserObject(user, _userService.Scope.officeId);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
        {

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (result.Succeeded)
            {
                _userService.Scope = new EntityScope { officeId = user.MainOfficeId };
                return CreateUserObject(user, _userService.Scope.officeId);
            }

            return Unauthorized();
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));


            return CreateUserObject(user, _userService.Scope.officeId);
        }

        private UserDTO CreateUserObject(AppUser user, int officeId)
        {
            var userDto = new UserDTO
            {
                CurrentOfficeId = officeId,
                DisplayName = user.FirstName + " " + user.LastName,
                //Image = null,
                Token = _tokenService.CreateToken(user),
                Username = user.UserName,

            };


            return userDto;
        }
    }
}