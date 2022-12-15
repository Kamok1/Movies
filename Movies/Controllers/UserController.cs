using Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.User;

namespace Movies.Controllers;
[ApiController]
[Authorize(Policy = "EverybodyAuthorized")]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMovieService _movieService;
    public UserController(IUserService userService, IMovieService movieService)
    {
        _userService = userService;
        _movieService = movieService;
    }

    [HttpGet]
    [Route("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        return Ok(await _userService.GetUserDto(httpContext: HttpContext));
    }


    [HttpPut]
    [Route("me/password")]
    public async Task<IActionResult> ChangeMyPassword([FromBody] PasswordChange passwords)
    {
        var user = await _userService.GetUserAsync(httpContext: HttpContext);
        await _userService.ChangePasswordAsync(user, passwords);
        return Ok();
    }

    [HttpPut]
    [Route("me/email")]
    public async Task<IActionResult> ChangeMyEmail([FromBody] EmailChange editEmail)
    {
        var user = await _userService.GetUserAsync(httpContext: HttpContext);
        await _userService.ChangeEmailAsync(user, editEmail);
        return Ok();
        //todo dorbić dobry serwis jakiś z potwierdzeniami
    }

    [HttpPut]
    [Route("me/profile")]
    public async Task<IActionResult> EditMyProfile([FromBody] EditUserProfile editProfile)
    {
        var user = await _userService.GetUserAsync(httpContext: HttpContext);
        await _userService.EditProfileAsync(user, editProfile);
        return Ok();
    }

    [HttpDelete]
    [Route("me")]
    public async Task<IActionResult> DeleteMe()
    {
        var user = await _userService.GetUserAsync(httpContext: HttpContext);
        await _userService.DeleteAsync(user);
        return Ok();
    }

    [HttpGet]
    [Route("me/movies")]
    public async Task<IActionResult> GetMyMovies()
    {
        var user = await _userService.GetUserAsync(httpContext: HttpContext);
        return Ok(await _movieService.GetUserMoviesAsync(user.Id));
    }

    [HttpDelete]
    [Route("me/movies/{movieId}")]
    public async Task<IActionResult> DeleteFromMyMovies([FromRoute] int movieId)
    {
        var user = await _userService.GetUserAsync(httpContext: HttpContext);
        await _movieService.DeleteFromUserMovies(user, movieId);
        return Ok();
    }

    [HttpPut]
    [Route("me/movies/{movieId}")]
    public async Task<IActionResult> AddToMyMovies([FromRoute] int movieId)
    {
        var user = await _userService.GetUserAsync(httpContext: HttpContext);
        await _movieService.AddUserMovieAsync(user, movieId);
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser([FromRoute] int userId)
    {
        var user = await _userService.GetUserAsync(userId);
        await _userService.DeleteAsync(user);
        return Ok();
    }

    [HttpGet]
    [Route("{userId}/movies")]
    public async Task<IActionResult> GetUserMovies([FromRoute] int userId)
    {
        await _movieService.GetUserMoviesAsync(userId);
        return Ok();
    }

    [HttpGet]
    [Route("{userId}/profile")]
    public async Task<IActionResult> GetProfile([FromRoute] int userId)
    {
        return Ok(await _userService.GetUserDto(userId));
    }
}