using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.DTOs;
using API.Middleware;
using API.Services.Interfaces;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly AppSettings _appSettings;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public TokenService(
            IOptions<AppSettings> appSettings,
            SignInManager<AppUser> signInManager,
            IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _appSettings = appSettings.Value;

        }
        public async Task<AuthenticateResponse> Authenticate(LoginDTO loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Email);
            var appUserOfficeRole = await _unitOfWork.Users.getAppUserOfficeRoleByUserAndOffice(user.Id, user.MainOfficeId);
            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user, user.MainOfficeId, appUserOfficeRole.Role.RoleName);

            return new AuthenticateResponse(user, token);
        }
        public string generateJwtToken(AppUser user, int officeId, string roleName)
        {
            var claims = new List<Claim>{
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("officeId", officeId.ToString()),
                new Claim("roleName", roleName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDscriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDscriptor);

            return tokenHandler.WriteToken(token);
        }


    }
}