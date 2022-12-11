using Data.Models;
using Models.Movie;

namespace Abstractions;

public interface IMovieService
{
    Task<DtoMovie> GetMovieDtoAsync(int id);
    Task<List<DtoMovie>> GetMoviesDtoAsync(int? year = null, string? title = null, int? genreId = null,
        int? directorId = null, int? actorId = null);
    Task<Movie> GetMovieAsync(int id);
    DtoMovie GetRandomDtoMovie(int? year = null, string? title = null, int? genreId = null,
        int? directorId = null, int? actorId = null);
    IQueryable<Movie> GetMoviesQuery(int? year = null, string? title = null, int? genreId = null, int? directorId = null,
        int? actorId = null, bool includeReviews = true, bool includeActors = false, bool includePoster = true, bool asNoTracking = false);
    Task<Movie> AddAsync(RequestMovie model);
    Task<Movie> EditAsync(RequestMovie model, int movieId);
    Task<bool> Delete(int id);
    Task<List<Movie>> GetUserMoviesAsync(int id);
    Task<bool> AddUserMovieAsync(User user, int movieId);
    Task<bool> DeleteFromUserMovies(User user, int movieId);
}
