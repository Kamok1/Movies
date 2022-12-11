using Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Actor;
using Models.Movie;

namespace Movies.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
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
        if (actorId != 0)
            return Ok(_actorService.GetActor(actorId));

        return Ok(_actorService.GetActors(movieId));
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromRoute] int actorId)
    {
        await _actorService.Delete(actorId);
        return Ok();
    }

    [HttpPut]
    [Route("{actorId}")]
    public async Task<IActionResult> Edit(RequestActor editActor, int actorId)
    {
        await _actorService.EditAsync(editActor, actorId);
        return Ok();
    }

    [HttpPut("movie")]
    public async Task<IActionResult> EditMovieActors(EditMovieActors editMovieActors)
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