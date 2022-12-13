using Abstractions;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.Exceptions;
using Models.Images;
using Models.Settings;

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

    public async Task<DtoImage> GetMainPosterDto(int movieId)
    {
        var poster = await _db.Poster.Where(poster => poster.Movie.Id == movieId && poster.IsMain).FirstOrDefaultAsync();
        return poster != default ? new DtoImage(poster.Path) : new DtoImage("poster/empty.jpg");
    }

    public async Task<List<DtoImage>> GetPostersDto(int movieId)
    {
        return await _db.Poster.Where(poster => poster.Movie.Id == movieId).Select(poster => new DtoImage(poster.Path)).ToListAsync();
    }

    public async Task<List<DtoImage>> GetPicturesDto(int movieId)
    {
        return await _db.Picture.Where(image => image.Movie.Id == movieId).Select(image => new DtoImage(image.Path)).ToListAsync();
    }
    public async Task AddPosterAsync(Movie movie, IFormFile file, bool isMain)
    {
        await _db.Entry(movie).Collection(m => m.Posters).LoadAsync();

        var folderName = $"{movie.Title}_{movie.Id}_posters";
        var folderPath = Path.Combine(_settings.ResourcesPath, _settings.PostersPath, folderName);
        var fileName = $"{movie.Posters.Count + 1}.jpg";

        var saveFileTask = _fileService.SaveFile(file, fileName, folderPath);

        if (isMain)
            movie.Posters.ForEach(poster => poster.IsMain = false);
        
        movie.Posters.Add(new Poster
        {
            Movie = movie,
            IsMain = isMain,
            Path = Path.Combine(_settings.PostersPath, folderName, fileName),
        });

        await saveFileTask;
        //usuwanie zdjec jak nie przejdzie dodawania filmu
        if (await _db.SaveChangesAsync() == 0)
            throw new AddingException<Poster>();
    }
    public void DeleteAllImages(Movie movie)
    {
        var folderName = $"{movie.Title}_{movie.Id}";

        Directory.Delete(Path.Combine(_settings.ResourcesPath, _settings.PostersPath, $"{folderName}_posters"), true);
        Directory.Delete(Path.Combine(_settings.ResourcesPath, _settings.PicturesPath, $"{folderName}_pictures"), true);
    }
    public async Task AddPictureAsync(Movie movie, IFormFile file)
    {
        await _db.Entry(movie).Collection(m => m.Pictures).LoadAsync();

        var folderName = $"{movie.Title}_{movie.Id}_pictures";
        var folderPath = Path.Combine(_settings.ResourcesPath, _settings.PicturesPath, folderName);
        var fileName = $"{movie.Pictures.Count + 1}.jpg";
        var saveFileTask = _fileService.SaveFile(file, fileName, folderPath);

        movie.Pictures.Add(new Picture
        {
            Movie = movie,
            Path = Path.Combine(_settings.PicturesPath, folderName, fileName),
        });
        await saveFileTask;
        if (await _db.SaveChangesAsync() == 0)
            throw new AddingException<Picture>();
    }
    public async Task EditMainPoster(string path)
    {
        var poster = await GetPoster(path);
        
        var movie = await _db.Movie.Where(movie => movie.Posters.Contains(poster)).Include(movie => movie.Posters)
            .FirstOrDefaultAsync();
        if (movie == default)
            throw new NotFoundException<Movie>();

        if (movie.Posters.Any(p => p.IsMain && p.Path == path))
            return;

        movie.Posters.ForEach(poster => poster.IsMain = poster.Path == path);
        if (await _db.SaveChangesAsync() == 0)
            throw new EditingException<Poster>();
    }
    public async Task DeletePictureAsync(string path)
    {
        var picture = await GetPicture(path);
        _db.Picture.Remove(picture);
        if (await _db.SaveChangesAsync() == 0)
            throw new DeletingException<Picture>();
    }
    public async Task DeletePosterAsync(string path)
    {
        var poster = await GetPoster(path);
        _db.Poster.Remove(poster);
        if (await _db.SaveChangesAsync() == 0)
            throw new DeletingException<Poster>();
    }

    private async Task<Poster> GetPoster(string path)
    {
        return await _db.Poster.FindAsync(path) ?? throw new NotFoundException<Poster>(); ;
    }
    private async Task<Picture> GetPicture(string path)
    {
        return await _db.Picture.FindAsync(path) ?? throw new NotFoundException<Picture>(); ;
    }
}