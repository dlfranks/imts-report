using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public enum Roles
    {
        Administrator = 1,
        User
    }
    public class Role
    {
        public int id { get; set; }
        public string roleName { get; set; }
    }
	
	
	public class AppUserRole
    {
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public string AppuserId { get; set; }
        public virtual AppUser AppUser { get; set; }
        //Users have roles by office
        public int ImtsOfficeId { get; set; }
        
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