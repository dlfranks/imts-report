using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Services.Models;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace API.Services.Interfaces
{
    public interface IUserManagerService
    {
        UserSetting CurrentUserSetting { get; }
        Task<AppUser> UserExists(string userName);
        string GetUserPassword(string userName);

        Task<bool> ValidateUser(string userName, string password);
        bool DeleteUser(string userName);
        Task<IdentityResult> CreateUser(AppUser user, string password);

        Task<bool> ChangePassword(AppUser user, string oldPassword, string newPassword);
        Task<IdentityResult> ResetPassword(AppUser user, string token, string newPassword);
    }

    public interface ITokenService
    {
        Task<AuthenticateResponse> Authenticate(LoginDTO loginDto);
        string generateJwtToken(AppUser user, int officeId);
    }
    public interface IUserService
    {
        Task<UserSetting> CreateUserSettings();
        void RemoveUserSetting();
    }
}