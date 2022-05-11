using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public interface IEntity
    {
        int Id { get; }
    }
    public interface IEntityScope
    {
        int officeId { get; set; }
    }
    public enum Roles
    {
        Administrator = 1,
        User
    }
    public class EntityScope : IEntityScope
    {
        public int officeId { get; set; }
    }
    public class OfficeRole : IEntity
    {
        public int Id { get; set; }

        public string RoleName { get; set; }
    }
	
	public class AppUserOfficeRole
    {
        public int RoleId { get; set; }
        public virtual OfficeRole Role { get; set; }
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