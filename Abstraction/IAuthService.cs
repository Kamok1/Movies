using Data.Migrations;
using Data.Models;
using Models.Auth;

namespace Abstractions;

public interface IAuthService
{
  Task<JwtResponse> GetJwtAsync(User user, string ip);
  void InvalidTokenHandler(User user);
  void ResetAllRefreshTokens(User user);
  bool ValidateRefreshToken(User user, string refreshToken);

}