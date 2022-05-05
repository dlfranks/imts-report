using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public enum UserRoles
    {
        Administrator = 1,
        Member,
        User
    }
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int MainOfficeId { get; set; }
        public bool IsImtsUser { get; set; }
        public string ImtsUserName { get; set; }
        public int ImtsEmployeeId { get; set; }

    }
}