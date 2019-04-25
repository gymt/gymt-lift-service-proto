using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace AuthService.Domain.Services
{
    public class UserService
    {
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly TokenService _tokenService;

        public UserService(
            SignInManager<CognitoUser> signInManager,
            TokenService tokenService)
        {
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        // TODO: handle failure (what to return?)
        public async Task<string> Authenticate(string Username, string Password)
        {
            var result = await _signInManager.PasswordSignInAsync(Username, Password, true, true);
            if (result.Succeeded)
            {
                // TODO: pass claims to GenerateToken
                var token = _tokenService.GenerateToken();

                return token;
            }
        }
    }
}
