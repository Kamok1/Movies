using Abstractions;
using Data;
using Data.Models;
using Models.Director;

namespace Implementations;

public class DirectorService : IDirectorService
{
    private readonly AppDbContext _db;

    public DirectorService(AppDbContext db)
    {
        _db = db;
    }
    public async Task<Director?> EditAsync(RequestDirector editDirector, int directorId)
    {
        var director = await _db.Director.FindAsync(directorId);
        if (director == null)
            return null;

        director.Name = editDirector.Name;
        director.Description = editDirector.Description;
        director.DateOfBirth = editDirector.DateOfBirth;
        return await _db.SaveChangesAsync() != 0 ? director : null;
    }

    public async Task<Movie?> EditMovieDirectorAsync(Movie movie, EditMovieDirector editMovieDirector)
    {
        var director = await _db.Director.FindAsync(editMovieDirector.DirectorId);
        if (director == null)
            return null;

        movie.Director = director;
        return await _db.SaveChangesAsync() != 0 ? movie : null;
    }

    public async Task<Director?> AddAsync(RequestDirector addDirector)
    {
        var model = new Director()
        {
            Name = addDirector.Name,
            Description = addDirector.Description,
            DateOfBirth = addDirector.DateOfBirth,
        };
        await _db.Director.AddAsync(model);
        return await _db.SaveChangesAsync() != 0 ? model : null;
    }

    public IQueryable<Director> GetDirectories(int? id = default)
    {
        var query = _db.Director.AsQueryable();
        if ((id ?? 0) != 0)
            return query.Where(director => director.Id == id);
        return query;
    }
}