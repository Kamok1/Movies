using Data.Models;
using Models.Director;

namespace Abstractions;

public interface IDirectorService
{
    Task<Director?> EditAsync(RequestDirector editDirector, int directorId);
    Task<Movie?> EditMovieDirectorAsync(Movie movie, EditMovieDirector editMovieDirector);
    Task<Director?> AddAsync(RequestDirector addDirector);
    IQueryable<Director> GetDirectories(int? id = default);
}