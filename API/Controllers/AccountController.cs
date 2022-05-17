using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs;
using API.Middleware;
using API.Services;
using API.Services.Interfaces;
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
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;


        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManger,
            ITokenService tokenService,
            UserService userService

            ) : base(userService)
        {
            _signInManager = signInManger;
            _tokenService = tokenService;
            _userManager = userManager;
        }
        [API.Middleware.Authorize]
        [HttpGet("switchoffice")]
        public async Task<ActionResult<UserDTO>> SwitchOffice(int newOfficeId)
        {
            var userId = HttpContext.Items["UserId"].ToString();
            var currentUserOfficeId = int.Parse(HttpContext.Items["OfficeId"].ToString());
            
            if (string.IsNullOrEmpty(userId) && newOfficeId <= 0)
                return BadRequest();
            
            var user = await _userManager.Users.Where(q => q.Id == userId).FirstOrDefaultAsync();
            var isUserInOffice = (await _userService.CreateUserSettings()).isUserInOffice(newOfficeId);
            if(user !=null && isUserInOffice) 
                CreateUserObject(user, newOfficeId);
            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
        {

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (result.Succeeded)
            {

                return CreateUserObject(user, user.MainOfficeId);
            }

            return Unauthorized();
        }
        

        [API.Middleware.Authorize]
        [HttpGet("getcurrentuser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser(int officeId)
        {
            var userId = HttpContext.Items["UserId"].ToString();
            var currentUserOfficeId = int.Parse(HttpContext.Items["OfficeId"].ToString());
            
            
            if (string.IsNullOrEmpty(userId) && officeId <= 0)
                return BadRequest();
            if(currentUserOfficeId != officeId) return Unauthorized("Current User Office is Not Valid.");
            var user = await _userManager.Users.Where(q => q.Id == userId).FirstOrDefaultAsync();
            var isUserInOffice = (await _userService.CreateUserSettings()).isUserInOffice(officeId);
            if(user !=null && isUserInOffice) 
                CreateUserObject(user, officeId);
            


            return BadRequest();
        }

        private UserDTO CreateUserObject(AppUser user, int officeId)
        {
            var userDto = new UserDTO
            {
                CurrentOfficeId = officeId,
                DisplayName = user.FirstName + " " + user.LastName,
                //Image = null,
                Token = _tokenService.generateJwtToken(user, officeId),
                Username = user.UserName,

            };


            return userDto;
        }
    }
}