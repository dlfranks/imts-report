using Application.User;

namespace Application.Interfaces
{
    public interface IUserAccessor
    {
        string GetUserId();
        int GetOfficeId();
        UserSetting GetUserSettings();
        void RemoveUserSetting();
    }
}