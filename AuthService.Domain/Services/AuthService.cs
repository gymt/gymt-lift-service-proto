using System;
using System.Threading.Tasks;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using AuthService.Controller.Services;
using Microsoft.AspNetCore.Identity;
using AuthService.Contract;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace AuthService.Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly CognitoUserPool _pool;
        private readonly CognitoUserManager<CognitoUser> _userManager;

        public AuthService(
            SignInManager<CognitoUser> signInManager,
            CognitoUserPool pool,
            UserManager<CognitoUser> userManager)
        {
            _signInManager = signInManager;
            _pool = pool;
            _userManager = userManager as CognitoUserManager<CognitoUser>;
        }

        public async Task<AuthResponse> Authenticate(string Username, string Password)
        {
            var user = await _userManager.FindByNameAsync(Username);

            if (user == null)
            {
                return null;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, Password, false);

            if (result.Succeeded)
            {
                return new AuthResponse()
                {
                    AccessToken = user.SessionTokens?.IdToken,
                    RefreshToken = user.SessionTokens?.RefreshToken
                };
            }

            return null;
        }

        public async Task<bool> Register(string Username, string Email, string Password)
        {
            var user = _pool.GetUser(Username);
            user.Attributes.Add(CognitoAttribute.Email.AttributeName, Email);

            var result = await _userManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return true;
            }
            else if (result.Errors.Count() > 0)
            {
                return false;
            }

            return false;
        }
    }
}
