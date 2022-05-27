using Application.User;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IUserAccessor
    {
        IHttpContextAccessor GetHttpContextAccessor ();
        string GetUserId();
        int GetOfficeId();
        UserSetting GetUserSettings();
        void RemoveUserSetting();
    }
}