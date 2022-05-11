using System.Collections.Generic;
using Domain.imts;

namespace API.DTOs
{
    public class UserDTO
    {
        public int CurrentOfficeId { get; set;}
        public string DisplayName { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
        public List<IDValuePair> MemberOffices{ get; set; }
    }
}