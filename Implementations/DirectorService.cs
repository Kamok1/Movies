using Abstractions;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Models.Director;
using Models.Exceptions;

namespace Implementations;

public class DirectorService : IDirectorService
{
    private readonly AppDbContext _db;

    public DirectorService(AppDbContext db)
    {
        _db = db;
    }
    public async Task EditAsync(RequestDirector editDirector, int directorId)
    {
        var director = await GetDirectorAsync(directorId);

        director.Name = editDirector.Name;
        director.Description = editDirector.Description;
        director.DateOfBirth = editDirector.DateOfBirth;
        if (await _db.SaveChangesAsync() == 0)
            throw new EditingException<Director>();
    }
    public async Task EditMovieDirectorAsync(Movie movie, int directorId)
    {
        var director = await GetDirectorAsync(directorId);

        movie.Director = director;
        if (await _db.SaveChangesAsync() == 0)
            throw new EditingException<Movie>();
    }

    public async Task AddAsync(RequestDirector addDirector)
    {
        var director = new Director()
        {
            Name = addDirector.Name,
            Description = addDirector.Description,
            DateOfBirth = addDirector.DateOfBirth,
        };
        await _db.Director.AddAsync(director);
        if (await _db.SaveChangesAsync() == 0)
            throw new AddingException<Director>();
    }

    public async Task DeleteAsync(int id)
    {
        _db.Remove(await GetDirectorAsync(id));
        if (await _db.SaveChangesAsync() == 0)
            throw new DeletingException<Director>();
    }

    public async Task<List<DtoDirector>> GetDirectorsDtoAsync()
    {
        return await _db.Director.Select(director => new DtoDirector(director)).ToListAsync();
    }

    public async Task<DtoDirector> GetDirectorDtoAsync(int id)
    {
        return new DtoDirector(await GetDirectorAsync(id));
    }

    private async Task<Director> GetDirectorAsync(int id)
    {
        var director = await _db.Director.FindAsync(id);
        if (director == default)
            throw new NotFoundException<Director>();
        return director;
    }
}