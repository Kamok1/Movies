using Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Director;
using Models.Movie;

namespace Movies.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
public class DirectorController : ControllerBase
{
    private readonly IDirectorService _directorService;
    private readonly IMovieService _movieServices;

    public DirectorController(IDirectorService directorService, IMovieService movieServices)
    {
        _directorService = directorService;
        _movieServices = movieServices;
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("{directorId}")]
    public async Task<IActionResult> Get([FromRoute] int directorId)
    {
        var director = await _directorService.GetDirectories(directorId).FirstOrDefaultAsync();
        return director != default ? Ok(new DtoDirector(director)) : NotFound();
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var directors = await _directorService.GetDirectories().ToListAsync();
        if (directors.IsNullOrEmpty())
            return NotFound();

        return Ok(directors.Select(director => new DtoDirector(director)));
    }

    [HttpPost]
    public async Task<IActionResult> Add(RequestDirector addDirector)
    {
        var director = await _directorService.AddAsync(addDirector);
        return director != default ? Ok(new DtoDirector(director)) : NotFound();
    }

    [HttpPut]
    [Route("{directorId}")]
    public async Task<IActionResult> Edit(RequestDirector editDirector, int directorId)
    {
        var director = await _directorService.EditAsync(editDirector, directorId);
        return director != default ? Ok(new DtoDirector(director)) : NotFound();
    }

    [HttpPut("movie")]
    public async Task<IActionResult> EditMovieDirectors(EditMovieDirector editMovieDirectors)
    {
        var movie = await _movieServices.GetMovieAsync(editMovieDirectors.MovieId);
        movie = await _directorService.EditMovieDirectorAsync(movie, editMovieDirectors);
        return movie != default ? Ok(new DtoMovie(movie)) : NotFound();
    }
}

