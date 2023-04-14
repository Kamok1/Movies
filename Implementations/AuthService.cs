using Abstractions;
using Data.Models;
using Models.Auth;
using Models.Settings;
using System.Security.Cryptography;
using Data;
using Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Implementations;

public class AuthService : IAuthService
{
  private readonly JwtSettings _jwtSettings;
  private readonly AppDbContext _db;
  public AuthService(JwtSettings jwtSettings, AppDbContext db)
  {
    _jwtSettings = jwtSettings;
    _db = db;
  }

  public async Task<JwtResponse> GetJwtAsync(User user)
  {
    var token = JwtService.GetToken(user, _jwtSettings);
    var refreshToken = GetRefreshToken(user);
    var jwtResponse = new JwtResponse();
    jwtResponse.RefreshToken = refreshToken;
    jwtResponse.Token = token;

    await SetRefreshTokenPropertiesAsync(user.Id, refreshToken.Token, refreshToken.Expires);

    return jwtResponse;
  }

  public bool ValidateRefreshToken(User user, string refreshToken)
  {
    return user.RefreshToken.Expires > DateTime.UtcNow;
  }
  
  public async Task InvalidTokenHandlerAsync(User user)
  {
    await SetRefreshTokenPropertiesAsync(user.Id ,string.Empty, DateTime.MinValue);
  }

  private async Task SetRefreshTokenPropertiesAsync(int userId, string newToken, DateTime newDate)
  {
    var dbUser = await _db.User.FindAsync(userId);
    if (dbUser == null)
      throw new NotFoundException<User>();
    dbUser.RefreshToken.Token = newToken;
    dbUser.RefreshToken.Expires = newDate;
    if (await _db.SaveChangesAsync() == 0)
      throw new EditingException<User>();
  }

  private UserRefreshToken GetRefreshToken(User user)
  {
    return new UserRefreshToken()
    {
      Expires = DateTime.UtcNow.AddSeconds(_jwtSettings.RefreshTokenExpire),
      Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
    };
  }

}