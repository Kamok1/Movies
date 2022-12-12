using Abstractions;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Models.Exceptions;
using Models.Images;
using Models.Settings;
using static MyExtensions;

namespace Implementations;

public class ImageService : IImageService
{
    private readonly AppDbContext _db;
    private readonly AppSettings _settings;
    private readonly IFileService _fileService;
    public ImageService(AppDbContext db, AppSettings settings, IFileService fileService)
    {
        _db = db;
        _settings = settings;
        _fileService = fileService;
    }

    public async Task<DtoImage> GetMainPoster(int movieId)
    {
        var poster = await _db.Poster.Where(poster => poster.Movie.Id == movieId && poster.IsMain).FirstOrDefaultAsync();
        return poster != null ? new DtoImage(poster.Path) : new DtoImage("poster/empty.jpg");
    }

    public async Task<List<DtoImage>> GetPosters(int movieId)
    {
        return await _db.Poster.Where(poster => poster.Movie.Id == movieId).Select(poster => new DtoImage(poster.Path)).ToListAsync();
    }

    public async Task<List<DtoImage>> GetImages(int movieId)
    {
        return await _db.Picture.Where(image => image.Movie.Id == movieId).Select(image => new DtoImage(image.Path)).ToListAsync();
    }
    public async Task<bool> AddPosterAsync(Movie movie, IFormFile file, bool isMain)
    {
        await _db.Entry(movie).Collection(m => m.Posters).LoadAsync();

        var folderName = $"{movie.Title}_{movie.Id}_posters";
        var folderPath = Path.Combine(_settings.ResourcesPath, _settings.PostersPath, folderName);
        var fileName = $"{movie.Posters.Count + 1}.jpg";

        var saveFileTask = _fileService.SaveFile(file, fileName, Path.Combine(folderPath, fileName));

        if (isMain)
            movie.Posters.ForEach(poster => poster.IsMain = false);
        
        movie.Posters.Add(new Poster
        {
            Movie = movie,
            IsMain = isMain,
            Path = Path.Combine(_settings.PostersPath, folderName, fileName),
        });

        await saveFileTask;
        return await _db.SaveChangesAsync() != 0;
    }

    public void DeleteAllImages(Movie movie)
    {
        var folderName = $"{movie.Title}_{movie.Id}";

        Directory.Delete(Path.Combine(_settings.ResourcesPath, _settings.PostersPath, $"{folderName}_posters"), true);
        Directory.Delete(Path.Combine(_settings.ResourcesPath, _settings.PicturesPath, $"{folderName}_pictures"), true);
    }
    public async Task<bool> AddPictureAsync(Movie movie, IFormFile file)
    {
        await _db.Entry(movie).Collection(m => m.Pictures).LoadAsync();

        var folderName = $"{movie.Title}_{movie.Id}_pictures";
        var folderPath = Path.Combine(_settings.ResourcesPath, _settings.PicturesPath, folderName);
        var fileName = $"{movie.Pictures.Count + 1}.jpg";

        var saveFileTask = _fileService.SaveFile(file, fileName, Path.Combine(folderPath, fileName));



        movie.Pictures.Add(new Picture
        {
            Movie = movie,
            Path = Path.Combine(_settings.PicturesPath, folderName, fileName),
        });
        await saveFileTask;
        return await _db.SaveChangesAsync() != 0;
    }
    public async Task<bool> EditMainPoster(Movie movie, string path)
    {
        await _db.Entry(movie).Collection(m => m.Pictures).LoadAsync();

        movie.Posters.ForEach(poster => poster.IsMain = poster.Path == path);
        return await _db.SaveChangesAsync() != 0;
    }
    public async Task<bool> DeletePictureAsync(string path)
    {
        var picture = await _db.Picture.FindAsync(path);
        if (picture == null)
            throw new NotFoundException<Picture>();
        _db.Picture.Remove(picture);
        return await _db.SaveChangesAsync() != 0;
    }
    public async Task<bool> DeletePosterAsync(string path)
    {
        var poster = await _db.Poster.FindAsync(path);
        if (poster == null)
            throw new NotFoundException<Poster>();
        _db.Poster.Remove(poster);
        return await _db.SaveChangesAsync() != 0;
    }
}