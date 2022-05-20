using System.Threading.Tasks;
using API.DTOs;
using Application.User;
using Domain;

namespace API.Services.Interfaces
{
    public interface ITokenService
    {
        Task<AuthenticateResponse> Authenticate(LoginDTO loginDto);
        string generateJwtToken(AppUser user, int officeId);
    }

}