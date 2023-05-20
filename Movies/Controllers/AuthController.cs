using Abstractions;
using Extensions;
using Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Models.Auth;
using Models.Settings;
using Models.User;

namespace Movies.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public AuthController(IUserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(UserRegisterRequest reqUser)
    {
        await _userService.CreateAsync(reqUser);
        return Ok();
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginModel reqLogin)
    {
        var user = await _userService.GetUserAsync(login: reqLogin.Login);
        if (PasswordServices.VerifyPassword(reqLogin.Password, user.Password, user.PasswordSalt) == false)
            return Unauthorized();
        
        return Ok(await _authService.GetJwtAsync(user, HttpContext.Request.HttpContext.Connection.RemoteIpAddress!.ToString()));
    }

    [HttpPost]
    [Route("refreshToken")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshToken)
    {
      var ip = HttpContext.Request.HttpContext.Connection.RemoteIpAddress!.ToString();


      var user = await _userService.GetUserAsync(refreshToken: new UserRequestRefreshToken{ Ip = ip, Token = refreshToken.Token});
      if (_authService.ValidateRefreshToken(user, ip) == false)
      {
        _authService.InvalidTokenHandler(user);
        return Unauthorized();
      }

      return Ok(await _authService.GetJwtAsync(user, ip));
    } 
}
