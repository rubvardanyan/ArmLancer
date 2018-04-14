using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ArmLancer.API.Utils.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ArmLancer.API.Auth
{
    
    public class AuthHelper
    {
        private IConfiguration _config;

        public AuthHelper(IConfiguration config)
        {
            _config = config;
        }
        
        public static string GenerateToken(User user, AuthSettings settings)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: settings.Issuer,
                audience: settings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(settings.ExpiresInMins),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}