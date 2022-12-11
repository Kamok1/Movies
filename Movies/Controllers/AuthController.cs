using Abstractions;
using Implementations;
using Microsoft.AspNetCore.Mvc;
using Extensions;
using Microsoft.EntityFrameworkCore;
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
        return await _userService.CreateAsync(reqUser) != null ? Ok() : BadRequest();
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginModel reqLogin)
    {
        var user = await _userService.GetUsersQuery(login: reqLogin.Login).FirstOrDefaultAsync();
        if(user == null)
            return NotFound();

        if (PasswordServices.VerifyPassword(reqLogin.Password, user.Password, user.PasswordSalt) == false)
            return Unauthorized();

        return Ok(JwtService.GetToken(user, _jwtSettings));
    }
}