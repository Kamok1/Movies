using Data.Models;
using Models.Movie;

namespace Abstractions;

public interface IMovieService
{
    Task<DtoMovie> GetMovieDtoAsync(int id);
    Task<Movie> GetMovieAsync(int id);
    Task<List<DtoMovie>> GetMoviesDtoAsync(int year, string? title, int genreId, int directorId, int actorId, int page, int pageSize, string orderBy);
    Task<List<DtoMovie>> GetUserMoviesAsync(int id);
    Task<DtoMovie> GetRandomDtoMovie(int? year = null, string? title = null, int? genreId = null,
        int? directorId = null, int? actorId = null);
    Task AddAsync(RequestMovie model);
    Task EditAsync(RequestMovie model, int movieId);
    Task DeleteAsync(int id);
    Task AddUserMovieAsync(User user, int movieId);
    Task DeleteFromUserMovies(User user, int movieId);
}
