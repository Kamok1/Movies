using Data.Models;
using Models.Genre;

namespace Abstractions;

public interface IGenreService
{
    Task<bool> AddAsync(GenresRequest genre);
    Task<List<Genre>?> GetAllGenresAsync();
    Task<Movie?> EditMovieGenres(Movie movie, EditGenre genre);

}