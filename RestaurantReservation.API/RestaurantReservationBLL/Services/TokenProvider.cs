using Microsoft.IdentityModel.Tokens;
using RestaurantReservation.API.Entities;
using RestaurantReservation.API.RestaurantReservationBLL.Abstractions;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestaurantReservation.API.RestaurantReservationBLL.Services
{
    public class TokenProvider : ITokenProvider
    {
        public string GenerateToken(int userId, string role)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("myIssuerSigningKeyqwerty123myIssuerSigningKeyqwerty123myIssuerSigningKeyqwerty123"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };
            var token = new JwtSecurityToken(
                issuer: "myIssuer",
                audience: "myAudience",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
