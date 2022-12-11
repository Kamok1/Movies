using Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Movie;

namespace Movies.Controllers;

[ApiController]
//[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;
    private readonly IImageService _imageService;

    public MovieController(IMovieService movieService, IImageService imageService)
    {
        _movieService = movieService;
        _imageService = imageService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetMovies(int id, int year, string? title, int genreId, int directorId, int actorId)
    {
        if (id != 0)
            return Ok(await _movieService.GetMovieDtoAsync(id));
        return Ok(await _movieService.GetMoviesDtoAsync(year, title, genreId, directorId, actorId));
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("random")]
    public IActionResult GetRandomMovie(int year, int actorId, int genreId, int directorId)
    {
        return Ok(_movieService.GetRandomDtoMovie(year: year, genreId: genreId, directorId: directorId, actorId: actorId));
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] RequestMovie reqMovie)
    {
        var movie = await _movieService.AddAsync(reqMovie);
        return Ok();
    }

    [HttpPut]
    [Route("{movieId}")]
    public async Task<IActionResult> Edit([FromBody] RequestMovie movie, int movieId)
    {
        await _movieService.EditAsync(movie, movieId);
        return Ok();
    }


    [HttpDelete("{movieId}")]
    public async Task<IActionResult> DeleteMovie([FromRoute] int movieId)
    {
        var movie = await _movieService.GetMovieAsync(movieId);
        _imageService.DeleteAllImages(movie);
        return await _movieService.Delete(movieId) ? Ok() : NotFound();
    }
}