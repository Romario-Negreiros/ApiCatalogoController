using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiCatalogoController.DTOs;
using Microsoft.IdentityModel.Tokens;

namespace ApiCatalogoController.Services
{
    public class JwtService : IJwtService
    {
        public IConfiguration configuration;
        public JwtService(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public UserTokenDTO GenerateJwt(string userEmail)
        {

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userEmail),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Token expires two hours after generation
            var expiration = DateTime.Now.AddMinutes(120);

            var token = new JwtSecurityToken(issuer: configuration["Jwt:Issuer"], audience: configuration["Jwt:Audience"], claims: claims, expires: expiration, signingCredentials: credentials);

            return new UserTokenDTO
            {
                Authenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                Message = $"Token generated at {DateTime.Now:F}"
        }
        }
    }
}
