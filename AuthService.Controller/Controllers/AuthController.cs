using Microsoft.AspNetCore.Mvc;
using AuthService.Controller.Services;
using System.Threading.Tasks;
using AuthService.Contract;
using Microsoft.AspNetCore.Authorization;

namespace AuthService.Controller.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Authenticate([FromBody] LoginRequest loginRequest)
        {
            var authResponse = await _authService.Authenticate(loginRequest.Username, loginRequest.Password);

            if (authResponse == null)
            {
                return Unauthorized();
            }
            else
            {
                return Ok(authResponse);
            }            
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            var success = await _authService.Register(registerRequest.Username, registerRequest.Email, registerRequest.Password);

            if (success)
            {
                return Ok();
            }

            return Unauthorized();
        }
    }
}
