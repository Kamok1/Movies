using Data.Migrations;
using Data.Models;
using Models.Auth;

namespace Abstractions;

public interface IAuthService
{
  Task<JwtResponse> GetJwtAsync(User user);
  Task InvalidTokenHandlerAsync(User user);
  bool ValidateRefreshToken(User user, string refreshToken);

}