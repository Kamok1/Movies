using Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.General;
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
    public async Task<IActionResult> GetMovieReviews(int movieId, [FromQuery] Filtering filtering)

    {
        return Ok(await _reviewService.GetMovieReviewsAsync(movieId, filtering.Page, filtering.PageSize, filtering.OrderBy));
    }

    [HttpPost]
    [Route("{movieId}")]
    public async Task<IActionResult> AddReview([FromBody] RequestReview review, [FromRoute] int movieId)
    {
        var movie = await _movieService.GetMovieAsync(movieId);
        var user = await _userService.GetUserAsync(httpContext: HttpContext);
        await _reviewService.AddAsync(review, movie, user);
        return Ok();
    }

    [HttpPut]
    [Route("{reviewId}")]
    public async Task<IActionResult> Edit([FromBody] RequestReview review, [FromRoute] int reviewId)
    {
        var user = await _userService.GetUserAsync(httpContext: HttpContext);
        await _reviewService.EditAsync(review, reviewId, user);
        return Ok();
    }

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    [Route("{reviewId}")]
    public async Task<IActionResult> Delete([FromRoute] int reviewId)
    {
        await _reviewService.DeleteAsync(reviewId);
        return Ok();
    }
}