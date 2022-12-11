using Data.Models;
using Microsoft.IdentityModel.Tokens;
using Models.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Implementations;

public static class JwtService 
{
    public static IEnumerable<Claim> DecodeToken(string token)
    {
        return new JwtSecurityToken(token.Substring(7)).Claims;
    }

    public static string GetToken(User user, JwtSettings jwtSettings)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = jwtSettings.Audience,
            Issuer = jwtSettings.Issuer,
            Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Role,user.Role.Name),
                    new Claim("id", user.Id.ToString())
                }
            ),
            Expires = DateTime.UtcNow.AddMinutes(jwtSettings.Expire),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Key)), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}