using System.Security.Claims;
using Application.Interfaces;
using Application.User;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Security
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetOfficeId()
        {
            var userSetting = GetUserSettings();
            return userSetting.currentOfficeId;
        }

        public string GetUserId()
        {
            var userSetting = GetUserSettings();
            return userSetting.appUserId;
        }


        public UserSetting GetUserSettings()
        {
            return (UserSetting)_httpContextAccessor.HttpContext.Items["CurrentUserSettings"];
        }

        public void RemoveUserSetting()
        {
            _httpContextAccessor.HttpContext.Items["CurrentUserSettings"] = null;
        }
    }
}