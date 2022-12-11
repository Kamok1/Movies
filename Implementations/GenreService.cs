using Abstractions;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Models.Genre;

namespace Implementations;

public class GenreService : IGenreService
{
    private readonly AppDbContext _db;

    public GenreService(AppDbContext db)
    {
        _db = db;
    }
    public async Task<bool> AddAsync(GenresRequest genres)
    {
        var genresList = genres.GenreNames.Select(genre => new Genre{Name = genre}).ToList();
        await _db.Genre.AddRangeAsync(genresList);
        return await _db.SaveChangesAsync() != 0;
    }
    public async Task<List<Genre>?> GetAllGenresAsync()
    {
        return await _db.Genre.ToListAsync();
    }
    public async Task<Movie?> EditMovieGenres(Movie movie,EditGenre genres)
    {
        var genresNames = genres.GenreIds.Select(id => _db.Genre.Find(id)).Select(genre=> genre?.Name).ToList();
        var listOfGenres =  await GetListOfGenresAsync(genresNames);
        if (movie.Genres.Equals(listOfGenres))
            return movie;

        movie.Genres = listOfGenres;
        return await _db.SaveChangesAsync() != 0 ? movie : null;
    }
    private async Task<List<Genre>> GetListOfGenresAsync(List<string?> genresNames)
    {
        var genres = new List<Genre>();
        foreach (var name in genresNames)
        {
            var genre = await _db.Genre.FirstOrDefaultAsync(e => e.Name == name);
            if(genre != default)
                genres.Add(genre);
        }

        return genres;
    }
}