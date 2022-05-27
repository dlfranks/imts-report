namespace API.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        public int CurrentOfficeId { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }

    }
}