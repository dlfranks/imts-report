using System.Threading.Tasks;
using API.DTOs;
using Domain;

namespace API.Services.Interfaces
{
    public interface ITokenService
    {
        Task<AuthenticateResponse> Authenticate(LoginDTO loginDto);
        string generateJwtToken(AppUser user, int officeId, string roleName);
    }

}