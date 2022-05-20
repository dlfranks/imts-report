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
        public string ImtsEmployeeUserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Password must be complex")]
        public string Password { get; set; }
        [Required]
        public string RoleName { get; set; }
        
        
        public string UserName { get; set; }
       
       
        public string OfficeId { get; set; }
        
        

    }
    public class RegisterDTO
    {
        

    }
}