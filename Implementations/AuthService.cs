using System.Net.Sockets;
using System.Security.Authentication;
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

  public async Task<JwtResponse> GetJwtAsync(User user, string ip)
  {
    var token = JwtService.GetToken(user, _jwtSettings);
    var refreshToken = CreateRefreshToken(user);
    var jwtResponse = new JwtResponse();
    jwtResponse.RefreshToken = refreshToken;
    jwtResponse.Token = token;

    await SetRefreshTokenPropertiesAsync(user, refreshToken.Token,ip, refreshToken.Expires);

    return jwtResponse;
  }

  public bool ValidateRefreshToken(User user, string ip)
  {
    var refreshToken = user.RefreshTokens.FirstOrDefault(token => token.Ip == ip);
    if (refreshToken == null)
      throw new AuthorizationException();
    return refreshToken.Expires > DateTime.UtcNow;
  }
  
  public void InvalidTokenHandler(User user)
  {
    ResetAllRefreshTokens(user);
  }

  public void ResetAllRefreshTokens(User user)
  {
    user.RefreshTokens.ForEach(token =>
    {
      token.Expires = DateTime.MinValue;
      token.Token = string.Empty;
    });
  }

  private async Task SetRefreshTokenPropertiesAsync(User user, string newToken, string ip, DateTime newDate)
  {
    var refreshToken = user.RefreshTokens.FirstOrDefault(token => token.Ip == ip);
    if (refreshToken == null)
    {
      user.RefreshTokens.Add(new RefreshToken { Expires = newDate, Token = newToken, Ip = ip});
    }
    else
    {
      refreshToken.Token = newToken;
      refreshToken.Expires = newDate;
      refreshToken.Ip = ip;
    }

    if (await _db.SaveChangesAsync() == 0)
      throw new EditingException<User>();
  }
  
  private UserRefreshToken CreateRefreshToken(User user)
  {
    return new UserRefreshToken()
    {
      Expires = DateTime.UtcNow.AddSeconds(_jwtSettings.RefreshTokenExpire),
      Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
    };
  }

}