using HairSalonVN.Database.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HairSalonVN.API.Helpers
{
    public class JwtHelper
    {
        private readonly IConfiguration _cfg;
        public JwtHelper(IConfiguration cfg) => _cfg = cfg;

        public string GenerateAccessToken(User user)
        {
            var claims = new Claim[]
            {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email,          user.Email),
            new(ClaimTypes.Name,           user.FullName),
            new(ClaimTypes.Role,           user.Role),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_cfg["JwtSettings:SecretKey"]!));
            var creds = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);
            var exp = int.Parse(
                _cfg["JwtSettings:AccessTokenExpiryMinutes"]!);

            var token = new JwtSecurityToken(
                issuer: _cfg["JwtSettings:Issuer"],
                audience: _cfg["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(exp),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
            => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

}
