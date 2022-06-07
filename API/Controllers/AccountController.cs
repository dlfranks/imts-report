using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Extensions.Filters;
using API.Services.Interfaces;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IUserAccessor _userAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManger,
            ITokenService tokenService,
            IUserAccessor userAccessor,
            IUnitOfWork unitOfWork
            )
        {
            _signInManager = signInManger;
            _tokenService = tokenService;
            _userAccessor = userAccessor;
            _unitOfWork = unitOfWork;
            _userManager = userManager;

        }
        [API.Middleware.Authorize]
        [ServiceFilter(typeof(UserActionFilter))]
        [HttpGet("switchoffice")]
        public async Task<ActionResult<UserDTO>> SwitchOffice(int newOfficeId)
        {
            var cuurentUserSettings = _userAccessor.GetUserSettings();
            var userId = cuurentUserSettings.appUserId;

            if (string.IsNullOrWhiteSpace(cuurentUserSettings.appUserId) && newOfficeId <= 0)
                return BadRequest();

            var user = await _userManager.Users.Where(q => q.Id == userId).FirstOrDefaultAsync();
            var isUserInOffice = cuurentUserSettings.isUserInOffice(newOfficeId);
            if (user != null && isUserInOffice)
            {
                var appUserOfficeRole = await _unitOfWork.Users.getAppUserOfficeRoleByUserAndOffice(user.Id, user.MainOfficeId);
                if (appUserOfficeRole == null && string.IsNullOrWhiteSpace(appUserOfficeRole.Role?.RoleName))
                    return Unauthorized();
                else
                    return CreateUserObject(user, newOfficeId, appUserOfficeRole.Role.RoleName);
            }

            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
        {

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized();
            var appUserOfficeRole = await _unitOfWork.Users.getAppUserOfficeRoleByUserAndOffice(user.Id, user.MainOfficeId);
            if (appUserOfficeRole == null && string.IsNullOrWhiteSpace(appUserOfficeRole.Role?.RoleName))
                return Unauthorized();
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (result.Succeeded)
            {

                return CreateUserObject(user, user.MainOfficeId, appUserOfficeRole.Role.RoleName);
            }

            return Unauthorized();
        }


        [API.Middleware.Authorize]
        [ServiceFilter(typeof(UserActionFilter))]
        [HttpGet("getcurrentuser")]
        public async Task<ActionResult<object>> GetCurrentUser()
        {
            var currentUserSettings = _userAccessor.GetUserSettings();

            if (currentUserSettings == null)
                return Unauthorized("UserSetting Error");

            var user = await _userManager.Users.Where(q => q.Id == currentUserSettings.appUserId).FirstOrDefaultAsync();
            var isUserInOffice = currentUserSettings.isUserInOffice(currentUserSettings.currentOfficeId);
            if (user != null && isUserInOffice)
                return Ok(new
                {
                    Id = user.Id,
                    OfficeId = currentUserSettings.currentOfficeId,
                    DisplayName = user.FirstName + " " + user.LastName,
                    MemberOffices = currentUserSettings.memberOffices
                });

            return BadRequest();
        }

        private UserDTO CreateUserObject(AppUser user, int officeId, string roleName)
        {
            var userDto = new UserDTO
            {
                Id = user.Id,
                OfficeId = officeId,
                DisplayName = user.FirstName + " " + user.LastName,
                Token = _tokenService.generateJwtToken(user, officeId, roleName),
                Username = user.UserName,

            };


            return userDto;
        }
    }
}