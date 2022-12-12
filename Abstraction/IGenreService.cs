using Data.Models;
using Models.Genre;

namespace Abstractions;

public interface IGenreService
{
    Task AddAsync(GenresRequest genre);
    Task<List<DtoGenre>> GetGenresDtoAsync();
    Task EditMovieGenresAsync(Movie movie, EditGenre genre);

}