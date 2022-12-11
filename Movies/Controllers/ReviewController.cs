using Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Review;

namespace Movies.Controllers;

[ApiController]
[Authorize(Policy = "EverybodyAuthorized")]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;
    private readonly IMovieService _movieService;
    private readonly IUserService _userService;

    public ReviewController(IReviewService reviewService, IMovieService movieService, IUserService userService)
    {
        _reviewService = reviewService;
        _movieService = movieService;
        _userService = userService;
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("{movieId}")]
    public async Task<IActionResult> GetMovieReviews(int movieId, [FromQuery] int pageSize, [FromQuery] int page)

    {
        return Ok(await _reviewService.GetMovieReviewsAsync(movieId, page, pageSize));
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("count/{movieId}")]
    public async Task<IActionResult> CountMovieReviews([FromRoute] int movieId)
    {
        return Ok(await _reviewService.CountMovieReviewsAsync(movieId));
    }

    [HttpPost]
    [Route("{movieId}")]
    public async Task<IActionResult> AddReview([FromBody] RequestReview review, [FromRoute] int movieId)
    {
        var movie = await _movieService.GetMovieAsync(movieId);
        var user = await _userService.GetUser(httpContext: HttpContext);

        return Ok(await _reviewService.AddAsync(review, movie, user) != null);
    }

    [HttpPut]
    [Route("{reviewId}")]
    public async Task<IActionResult> Edit([FromBody] RequestReview review,[FromRoute] int reviewId)
    {
        var user = await _userService.GetUser(httpContext: HttpContext);

        return await _reviewService.EditAsync(review, reviewId, user) ? Ok() : Unauthorized();
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    [Route("{reviewId}")]
    public async Task<IActionResult> Delete([FromRoute] int reviewId)
    {
        return Ok(await _reviewService.DeleteAsync(reviewId));
    }
}