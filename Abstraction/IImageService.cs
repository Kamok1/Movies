using Data.Models;
using Microsoft.AspNetCore.Http;
using Models.Images;

namespace Abstractions;

public interface IImageService
{
    Task<DtoImage> GetMainPosterDtoAsync(int movieId);
    Task<List<DtoImage>> GetPostersDtoAsync(int movieId);
    Task<List<DtoImage>> GetPicturesDtoAsync(int movieId);
    void DeleteAllImages(Movie movie);
    Task AddPosterAsync(Movie movie, IFormFile poster, bool isMain);
    Task AddPictureAsync(Movie movie, IFormFile poster);
    Task EditMainPosterAsync(string path);
    Task DeletePictureAsync(string path);
    Task DeletePosterAsync(string path);
}