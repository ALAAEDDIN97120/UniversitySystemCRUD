using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using University.Core.DTOs;

namespace University.API.Helpers
{
    public class JwtTokenHelper : IjwtTokenHelper
    {
        private IConfiguration _config;
        public JwtTokenHelper(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateToken(UserDTO user)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha512Signature);
            
            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Email, user.Email ?? " "),
        new Claim(ClaimTypes.Role, "Student"),
    };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(2),
                SigningCredentials = creds,
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
    public interface IjwtTokenHelper
    {
        string GenerateToken(UserDTO user);
    }
}