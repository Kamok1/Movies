using Abstractions;
using Extensions;
using Implementations;
using Microsoft.AspNetCore.Mvc;
using Models.Auth;
using Models.Settings;
using Models.User;

namespace Movies.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly JwtSettings _jwtSettings;

    public AuthController(IUserService userService, JwtSettings jwtSettings)
    {
        _userService = userService;
        _jwtSettings = jwtSettings;
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
        
        return Ok(new JwtResponse()
                  {
                    Jwt = JwtService.GetToken(user, _jwtSettings),
                    ExpiresIn = _jwtSettings.Expire
                  });
    }
}
