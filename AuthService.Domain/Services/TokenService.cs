using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Domain.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            var secret = _config.GetValue<string>("Secret");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

            var jwt = new JwtSecurityToken(issuer: "PUMPT",
                audience: "Everyone",
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public string CreateRefreshToken()
        {
            var seed = new byte[32];
            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(seed);
                return Convert.ToBase64String(seed);
            }
        }
    }
}
