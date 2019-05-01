using System.Threading.Tasks;
using AuthService.Contract;

namespace AuthService.Controller.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> Authenticate(string Username, string Password);
        Task<bool> Register(string Username, string Email, string Password);
    }
}
