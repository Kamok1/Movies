using Abstractions;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Models.Exceptions;
using Models.Genre;

namespace Implementations;

public class GenreService : IGenreService
{
    private readonly AppDbContext _db;

    public GenreService(AppDbContext db)
    {
        _db = db;
    }
    public async Task AddAsync(GenresRequest genres)
    {
        var genresList = genres.GenreNames.Select(genre => new Genre { Name = genre }).ToList();
        await _db.Genre.AddRangeAsync(genresList);
        if (await _db.SaveChangesAsync() == 0)
            throw new AddingException<Genre>();
    }
    public async Task<List<DtoGenre>> GetGenresDtoAsync()
    {
        return await _db.Genre.Select(genre => new DtoGenre(genre)).ToListAsync();
    }
    public async Task EditMovieGenresAsync(Movie movie, EditGenre genres)
    {
        var listOfGenres = genres.GenreIds.Select(id => _db.Genre.Find(id)).ToList();
        if (movie.Genres.Equals(listOfGenres))
            return;

        movie.Genres = listOfGenres!;
        if (await _db.SaveChangesAsync() == 0)
            throw new EditingException<Genre>();
    }
}