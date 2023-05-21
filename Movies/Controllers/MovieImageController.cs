using Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Movies.Controllers;
[ApiController]
[Route("api/[controller]")]
public class MovieImageController : ControllerBase
{
    private readonly IMovieService _movieService;
    private readonly IImageService _imageService;

    public MovieImageController(IMovieService movieService, IImageService imageService)
    {

        _movieService = movieService;
        _imageService = imageService;
    }


    [HttpPost]
    [Route("{movieId}/poster")]
    public async Task<IActionResult> AddPoster([FromRoute] int movieId, [FromQuery] bool isMain, IFormFile poster)
    {
        if (poster.Length > 10485760) //10mb
        {
            ModelState.AddModelError("File", "File's size is too large");
            return ValidationProblem();
        }

        var movie = await _movieService.GetMovieAsync(movieId);
        await _imageService.AddPosterAsync(movie, poster, isMain);
        return Ok();
    }

    [HttpPost]
    [Route("{movieId}/image")]
    public async Task<IActionResult> AddImage([FromRoute] int movieId, IFormFile image)
    {
        var movie = await _movieService.GetMovieAsync(movieId);
        await _imageService.AddPictureAsync(movie, image);
        return Ok();
    }
    [HttpDelete]
    [Route("poster/{path}")]
    public async Task<IActionResult> DeletePoster([FromRoute] string path)
    {
        await _imageService.DeletePosterAsync(path);
        return Ok();
    }

    [HttpPut]
    [Route("poster/{path}")]
    public async Task<IActionResult> EditMainPoster([FromRoute] string path)
    {
        await _imageService.EditMainPosterAsync(path);
        return Ok();
    }


    [HttpDelete]
    [Route("image/{path}")]
    public async Task<IActionResult> DeleteImage([FromRoute] string path)
    {
        await _imageService.DeletePictureAsync(path.Replace(@"\\", @"\"));
        return Ok();
    }

    [HttpGet]
    [Route("{movieId}/image")]
    public async Task<IActionResult> GetImages([FromRoute] int movieId)
    {
        var res = await _imageService.GetPicturesDtoAsync(movieId);
        return Ok(res);
    }

    [HttpGet]
    [Route("{movieId}/poster")]
    public async Task<IActionResult> GetPosters([FromRoute] int movieId)
    {
        return Ok(await _imageService.GetPostersDtoAsync(movieId));
    }

    [HttpGet]
    [Route("{movieId}/poster/main")]
    public async Task<IActionResult> GetMainPoster([FromRoute] int movieId)
    {
        return Ok(await _imageService.GetMainPosterDtoAsync(movieId));
    }

}
