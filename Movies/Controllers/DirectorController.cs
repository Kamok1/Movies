using Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Director;

namespace Movies.Controllers;

[ApiController]
//[Authorize(Roles = "Admin")]
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
    public async Task<IActionResult> Get(int id)
    {
        if (id > 0)
            return Ok(await _directorService.GetDirectorDtoAsync(id));
        return Ok(await _directorService.GetDirectorsDtoAsync());
    }

    [HttpPut("photo/{id}")]
    public async Task<IActionResult> EditPhoto([FromRoute] int id, IFormFile picture)
    {
      await _directorService.EditActorPicture(picture, id);
      return Ok();
    }

  [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _directorService.DeleteAsync(id);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Add(RequestDirector addDirector)
    {
        await _directorService.AddAsync(addDirector);
        return Ok();
    }

    [HttpPut]
    [Route("{directorId}")]
    public async Task<IActionResult> Edit(RequestDirector editDirector, int directorId)
    {
        await _directorService.EditAsync(editDirector, directorId);
        return Ok();
    }

    [HttpPut("movie")]
    public async Task<IActionResult> EditMovieDirectors(EditMovieDirector editMovieDirectors)
    {
        var movie = await _movieServices.GetMovieAsync(editMovieDirectors.MovieId);
        await _directorService.EditMovieDirectorAsync(movie, editMovieDirectors.DirectorId);
        return Ok();
    }
}

