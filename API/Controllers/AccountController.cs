using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs;
using API.Services;
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
        private readonly TokenService _tokenService;
        private readonly UserService _userService;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManger,
            TokenService tokenService,
            UserService userService,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _tokenService = tokenService;
            _userService = userService;
            _signInManager = signInManger;
            _userManager = userManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (result.Succeeded)
            {
                _userService.Scope.officeId = user.MainOfficeId;
                return await CreateUserObject(user);
            }

            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Create(RegisterDTO registerDto)
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
                    return await CreateUserObject(appUserResult.Value);
                }

                
            }catch(Exception e)
            {
                var msg = "Internal error, unable to create a user" + e;
                //Elmah
            }
            return BadRequest("Problem registering user");
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            return await CreateUserObject(user);
        }

        private async Task<UserDTO> CreateUserObject(AppUser user)
        {
            var userDto = new UserDTO
            {
                MainOfficeId = user.MainOfficeId,
                CurrentOfficeId = scope.officeId,
                DisplayName = user.FirstName + " " + user.LastName,
                //Image = null,
                Token = _tokenService.CreateToken(user),
                Username = user.UserName,
                
            };
            userDto.MemberOffices = await _userService.GetUserOffices(user);
            return userDto;
        }
    }
}