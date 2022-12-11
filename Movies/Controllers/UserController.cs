using Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Movie;
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
        var user = await _userService.GetUsersQuery(httpContext: HttpContext).FirstOrDefaultAsync();
        return user == null ? NotFound() : Ok(new DtoUser(user));
    }


    [HttpPut]
    [Route("me/password")]
    public async Task<IActionResult> ChangeMyPassword([FromBody] PasswordChange passwords)
    {
        var user = await _userService.GetUsersQuery(httpContext: HttpContext).FirstOrDefaultAsync();
        return user == null ? NotFound() : Ok(await _userService.ChangePassword(user, passwords));
    }

    [HttpPut]
    [Route("me/email")]
    public async Task<IActionResult> ChangeMyEmail([FromBody] EmailChange editEmail)
    {
        var user = await _userService.GetUsersQuery(httpContext: HttpContext).FirstOrDefaultAsync();
        return user == null ? NotFound() : Ok(await _userService.ChangeEmail(user, editEmail));
        //todo dorbić dobry serwis jakiś z potwierdzeniami
    }

    [HttpPut]
    [Route("me/profile")]
    public async Task<IActionResult> EditMyProfile([FromBody] EditUserProfile editProfile)
    {
        var user = await _userService.GetUsersQuery(httpContext: HttpContext).FirstOrDefaultAsync();
        return user == null ? NotFound() : Ok(await _userService.EditProfile(user, editProfile));
    }

    [HttpDelete]
    [Route("me")]
    public async Task<IActionResult> DeleteMe()
    {
        var user = await _userService.GetUsersQuery(httpContext: HttpContext).FirstOrDefaultAsync();
        return user != null ? Ok(await _userService.DeleteAsync(user)) : NotFound();
    }

    [HttpGet]
    [Route("me/movies")]
    public async Task<IActionResult> GetMyMovies()
    {
        var user = await _userService.GetUsersQuery(httpContext: HttpContext).FirstOrDefaultAsync();
        if (user == null)
            return NotFound();

        var movies = await _movieService.GetUserMoviesAsync(user.Id);
        return Ok(movies.Select(movie => new DtoMovie(movie)));
    }

    [HttpDelete]
    [Route("me/movies/{movieId}")]
    public async Task<IActionResult> DeleteFromMyMovies([FromRoute] int movieId)
    {
        var user = await _userService.GetUsersQuery(httpContext: HttpContext).FirstOrDefaultAsync();
        if (user == null)
            return NotFound();

        var movies = await _movieService.DeleteFromUserMovies(user, movieId);
        return movies ? Ok() : NotFound();
    }

    [HttpPut]
    [Route("me/movies/{movieId}")]
    public async Task<IActionResult> AddToMyMovies([FromRoute] int movieId)
    {
        var user = await _userService.GetUsersQuery(httpContext: HttpContext).FirstOrDefaultAsync();
        return user != null ? Ok(await _movieService.AddUserMovieAsync(user, movieId)) : NotFound();
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("user/{userId}")]
    public async Task<IActionResult> DeleteUser([FromRoute] int userId)
    {
        var user = await _userService.GetUsersQuery(userId).FirstOrDefaultAsync();
        return user != default ? Ok(await _userService.DeleteAsync(user)) : NotFound();
    }

    [HttpGet]
    [Route("{userId}/movies")]
    public async Task<IActionResult> GetUserMovies([FromRoute] int userId)
    {
        var movies = await _movieService.GetUserMoviesAsync(userId);
        return Ok(movies.Select(movie => new DtoMovie(movie)));
    }

    [HttpGet]
    [Route("{idUser}/profile")]
    public async Task<IActionResult> GetProfile([FromRoute] int idUser)
    {
        var user = await _userService.GetUsersQuery(idUser).FirstOrDefaultAsync();
        return user == default ? Ok(new DtoUser(user!)) : NotFound();
    }
    
}