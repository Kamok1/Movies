using Data.Models;
using Microsoft.AspNetCore.Http;
using Models.Images;

namespace Abstractions;

public interface IImageService
{
    Task<DtoImage> GetMainPoster(int movieId);
    Task<List<DtoImage>> GetPosters(int movieId);
    Task<List<DtoImage>> GetImages(int movieId);
    void DeleteAllImages(Movie movie);
    Task<bool> AddPosterAsync(Movie movie, IFormFile poster, bool isMain);
    Task<bool> AddPictureAsync(Movie movie, IFormFile poster);
    Task<bool> EditMainPoster(Movie movie, string path);
    Task<bool> DeletePictureAsync(string path);
    Task<bool> DeletePosterAsync(string path);
}