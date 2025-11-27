using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using IPLStore.API.Identity;

namespace IPLStore.API.Services
{
    public class JwtTokenService
    {
        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken(ApplicationUser user)
        {
            // Claims inside the token
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),       // Subject = User ID
                new Claim(JwtRegisteredClaimNames.Email, user.Email),  // Email
                new Claim(ClaimTypes.Name, user.Email)                 // ASP.NET Core Identity Name claim
            };

            // Secret key
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            // Encryption algorithm
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create the token
            var token = new JwtSecurityToken(
                issuer: null,         // optional
                audience: null,       // optional
                claims: claims,
                expires: DateTime.UtcNow.AddHours(12),
                signingCredentials: creds
            );

            // Return token string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
