using Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Genre;
using Models.Movie;

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
    public async Task<IActionResult> GetGenres()
    {
        var genres = await _genreService.GetAllGenresAsync();
        return genres != null ? Ok(genres.Select(genre => new DtoGenre(genre))) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> AddGenre(GenresRequest genre)
    {
        return Ok(await _genreService.AddAsync(genre));
    }

    [HttpPut]
    [Route("movie")]
    public async Task<IActionResult> EditMovieGenres(EditGenre editGenre)
    {
        var movie = await _movieService.GetMovieAsync(editGenre.MovieId);
        movie = await _genreService.EditMovieGenres(movie, editGenre);

        return movie != null ? Ok(new DtoMovie(movie)) : BadRequest();
    }
}