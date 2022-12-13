using Data.Models;
using Microsoft.AspNetCore.Http;
using Models.Images;

namespace Abstractions;

public interface IImageService
{
    Task<DtoImage> GetMainPosterDto(int movieId);
    Task<List<DtoImage>> GetPostersDto(int movieId);
    Task<List<DtoImage>> GetPicturesDto(int movieId);
    void DeleteAllImages(Movie movie);
    Task AddPosterAsync(Movie movie, IFormFile poster, bool isMain);
    Task AddPictureAsync(Movie movie, IFormFile poster);
    Task EditMainPoster(string path);
    Task DeletePictureAsync(string path);
    Task DeletePosterAsync(string path);
}