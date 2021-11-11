using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsWoodEmployee { get; set; }

        public string DisplayName ()
        {
            return FirstName + " " + LastName;
        }

    }
}