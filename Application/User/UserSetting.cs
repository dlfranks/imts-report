using System.Collections.Generic;
using System.Linq;
using Domain.imts;

namespace Application.User
{
    public class UserSetting
    {
        public string appUserId { get; set; }
        public string roleName { get; set; }
        public int currentOfficeId { get; set; }
        public string userName { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public int? mainOfficeId { get; set; }
        public string mainOfficeName { get; set; }
        public bool IsImtsUser { get; set; }
        public int? imtsEmployeeId { get; set; }

        public bool isAuthenticated
        {
            get
            {
                return (!string.IsNullOrEmpty(appUserId));
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

        public bool isUserInOffice(int officeId)
        {
            if (!isAuthenticated)
                return (false);
            if (officeId <= 0)
                return (false);
            if (_memberOffices == null)
                return false;

            return _memberOffices.Where(q => q.id == officeId).Count() > 0;
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