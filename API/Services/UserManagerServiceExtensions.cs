using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;
using API.Services.Interfaces;
using API.Services.Models;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace API.Services.Models
{
    public class UserSetting
    {
        public string[] roles { get; set; }
        public string userName { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public int? officeId { get; set; }
        public string officeName { get; set; }
        public UnitSystem officeUnitSystem { get; set; }
        public OfficeRegion officeRegion { get; set; }
        public int? employeeId { get; set; }
        public bool _isProjectManager { get; set; }
        public bool isProjectManager
        {
            get
            {
                return (isAuthenticated && _isProjectManager);
            }
        }

        public bool _isProjectEngineer { get; set; }
        public bool isProjectEngineer
        {
            get
            {
                return (isAuthenticated && _isProjectEngineer);
            }
        }

        public bool isAuthenticated
        {
            get
            {
                return (employeeId.HasValue);
            }
        }

        public bool _isSuperUser { get; set; }
        public bool isSuperUser
        {
            get
            {
                return (isAuthenticated && _isSuperUser);
            }
        }

        public bool isInRole(string role)
        {
            if (!isAuthenticated)
                return (false);
            if (roles == null)
                return (false);
            if (String.IsNullOrWhiteSpace(role))
                return (false);
            return roles.Contains(role.ToLower());
        }

        public List<IDValuePair> _memberOffices { get; set; }
        public List<IDValuePair> memberOffices
        {
            get
            {
                return (_memberOffices);
            }
        }
    }

}
namespace API.Services
{

    public class UserManagerService : UserManager<AppUser>, IUserManagerService
    {
        public UserManagerService(
        IUserStore<AppUser> store,
        IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<AppUser> passwordHasher,
        IEnumerable<IUserValidator<AppUser>> userValidators,
        IEnumerable<IPasswordValidator<AppUser>> passwordValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        IServiceProvider services,
        ILogger<UserManager<AppUser>> logger)
        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {


        }

        public UserSetting CurrentUserSetting
        {
            get
            {
                return new UserSetting();
            }
        }

        public void SwitchOffice(int officeId)
        {
            CurrentUserSetting.officeId = officeId;
        }
        public int MinPasswordLength => throw new NotImplementedException();

        public async Task<bool> ChangePassword(AppUser user, string oldPassword, string newPassword)
        {
            if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
            if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");

            try
            {
                var result = await ChangePasswordAsync(user, oldPassword, newPassword);
                if (result.Succeeded)
                    return true;
                else
                    return false;
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IdentityResult> CreateUser(AppUser user, string password)
        {
            return await CreateAsync(user, password);

        }

        public bool DeleteUser(string userName)
        {
            return DeleteUser(userName);
        }

        public string GetUserPassword(string userName)
        {
            return GetUserPassword(userName);
        }

        public string ResetPassword(string userName)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            return ResetPassword(userName);
        }

        public async Task<bool> UserExists(string userName)
        {
            var user = await FindByNameAsync(userName);
            return user != null ? true : false;
        }

        public async Task<bool> ValidateUser(string userName, string password)
        {
            var valid = false;

            var user = await FindByNameAsync(userName);
            valid = await CheckPasswordAsync(user, password);
            return valid;
        }

        protected bool ImtsValidateUser(string userName, string password, string domain, out List<string> errors)
        {
            errors = new List<string>();
            var valid = false;
            return valid;
        }
    }
}