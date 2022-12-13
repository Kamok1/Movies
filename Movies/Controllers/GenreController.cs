using Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Genre;

namespace Movies.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class GenreController : ControllerBase
{
    private readonly IGenreService _genreService;
    private readonly IMovieService _movieService;

    public GenreController(IGenreService genreService, IMovieService movieService)
    {
        _genreService = genreService;
        _movieService = movieService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get()
    {
        return Ok(await _genreService.GetGenresDtoAsync());
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] GenresRequest genre)
    {
        await _genreService.AddAsync(genre);
        return Ok();
    }

    [HttpPut]
    [Route("movie")]
    public async Task<IActionResult> EditMovieGenres([FromBody] EditGenre editGenre)
    {
        var movie = await _movieService.GetMovieAsync(editGenre.MovieId);
        await _genreService.EditMovieGenresAsync(movie, editGenre);
        return Ok();
    }
}