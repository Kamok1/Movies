using Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Actor;
using Models.Settings;

namespace Movies.Controllers;

[ApiController]
//[Authorize(Roles = "Admin")]
[Route("api/[controller]")]

public class ActorController : ControllerBase
{
    private readonly IActorService _actorService;
    private readonly IMovieService _movieService;

    public ActorController(IMovieService movieService, IActorService actorService)
    {
        _movieService = movieService;
        _actorService = actorService;
    }


    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get(int actorId, int movieId)
    {
        if (actorId > 0)
            return Ok(await _actorService.GetActorDtoAsync(actorId));

        return Ok(await _actorService.GetActorsDtoAsync(movieId));
    }

    [HttpPut("photo/{id}")]
    public async Task<IActionResult> EditPhoto([FromRoute] int id, IFormFile picture)
    {
      await _actorService.EditActorPicture(picture, id);
      return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await _actorService.DeleteAsync(id);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Edit([FromBody] RequestActor editActor, [FromRoute] int id)
    {
        await _actorService.EditAsync(editActor, id);
        return Ok();
    }

    [HttpPut("movie")]
    //todo put into movie controller
    public async Task<IActionResult> EditMovieActors([FromBody] EditMovieActors editMovieActors)
    {
        var movie = await _movieService.GetMovieAsync(editMovieActors.MovieId);
        await _actorService.EditMovieActorsAsync(movie, editMovieActors);
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Add(RequestActor reqActor)
    {
        await _actorService.AddAsync(reqActor);
        return Ok();
    }
}