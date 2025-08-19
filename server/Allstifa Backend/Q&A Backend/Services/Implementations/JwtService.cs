using Microsoft.IdentityModel.Tokens;
using Q_A_Backend.Models;
using Q_A_Backend.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Q_A_Backend.Services.Implementations
{
    public class JwtService: IJwtService
    {
        private readonly IConfiguration _config;
        public JwtService(IConfiguration configuration)
        {
            _config = configuration;
        }

        public string  GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim (JwtRegisteredClaimNames.Name, user.Name),
                new Claim("role", user.Role),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
