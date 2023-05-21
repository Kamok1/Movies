using Data.Models;
using Microsoft.AspNetCore.Http;
using Models.Director;

namespace Abstractions;

public interface IDirectorService
{
    Task EditAsync(RequestDirector editDirector, int directorId);
    Task EditMovieDirectorAsync(Movie movie, int directorId);
    Task AddAsync(RequestDirector addDirector);
    Task DeleteAsync(int id);
    Task EditDirectorPhoto(IFormFile picture,  int id);
    Task<List<DtoDirector>> GetDirectorsDtoAsync();
    Task<DtoDirector> GetDirectorDtoAsync(int id);
}