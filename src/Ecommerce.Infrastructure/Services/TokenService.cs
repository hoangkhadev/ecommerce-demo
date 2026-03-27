using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Ecommerce.Infrastructure.Services
{
    public class TokenService(IOptions<JwtOptions> jwtOptions) : ITokenService
    {
        public string GenerateToken(Guid userId, string userName, string userRole)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.SecretKey));
            var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var idStr = userId.ToString();
            List<Claim> claims = [
                new(JwtRegisteredClaimNames.Sub, idStr),
                new(JwtRegisteredClaimNames.PreferredUsername, userName),
                new("role", userRole.ToString())
            ];

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(jwtOptions.Value.ExpirationInMinutes),
                SigningCredentials = credentials,
                Issuer = jwtOptions.Value.Issuer,
                Audience = jwtOptions.Value.Audience,
            };

            var tokenHandler = new JsonWebTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return token;
        }
    }
}
