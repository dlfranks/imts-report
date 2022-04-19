using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs;
using API.Services;
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
        private readonly TokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManger,
            TokenService tokenService)
        {
            _tokenService = tokenService;
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
                return CreateUserObject(user);
            }

            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Create(RegisterDTO registerDto)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            //permissionForEmployee()
            
            var claims = await _userManager.GetClaimsAsync(currentUser);
            var officeId = claims.FirstOrDefault(c => c.Type == "officeId");
            //validate an email
            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                ModelState.AddModelError("email", "Email taken");
                return ValidationProblem();
            }
            //Validate the username
            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Email))
            {
                ModelState.AddModelError("username", "Username taken");
                return ValidationProblem();
            }
            //Validate the user role for create a new user



            var user = new AppUser
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email,
                IsWoodEmployee = registerDto.IsWoodEmployee,
                MainOfficeId = officeId
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }

            return BadRequest("Problem registering user");
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));

            return CreateUserObject(user);
        }

        private UserDTO CreateUserObject(AppUser user)
        {
            return new UserDTO
            {
                OfficeId = user.MainOfficeId,
                DisplayName = user.FirstName + " " + user.LastName,
                //Image = null,
                Token = _tokenService.CreateToken(user),
                Username = user.UserName
            };
        }
    }
}