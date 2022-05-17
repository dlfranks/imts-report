namespace API.ViewModels
{
    public enum FormViewMode { View, Edit, Create, List };
    public class AppUserOfficeRoleViewModel
    {
        public string UserId { get; set; }
        public int OfficeId { get; set; }
        public string OfficeName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}