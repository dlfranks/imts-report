using System.ComponentModel.DataAnnotations;

namespace Application.User
{

    public class AppUserOfficeRoleViewModel
    {
        public string UserId { get; set; }
        public int OfficeId { get; set; }
        public string OfficeName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
    public class AppUserDTO
    {
        public string Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public bool IsImtsUser { get; set; } = false;
        public string ImtsUserName { get; set; }
        public int? ImtsEmployeeId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        [Required]
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public int MainOfficeId { get; set; }
        public bool IsNewUser { get; set; }
    }
    public class RegisterDTO
    {


    }

    public class LookupUserNameDTO
    {
        public bool isValidToCreate { get; set; }
        public AppUserDTO appUserDTO { get; set; }
        public string errmsg { get; set; }
        public string succmsg { get; set; }
    }
}