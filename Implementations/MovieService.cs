using Abstractions;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Models.Exceptions;
using Models.Movie;
using Models.Settings;

namespace Implementations
{
    public class MovieService : IMovieService
    {
        private readonly AppDbContext _db;
        private readonly AppSettings _settings;

        public MovieService(AppDbContext db, AppSettings settings)
        {
            _db = db;
            _settings = settings;
        }

        public async Task<List<DtoMovie>> GetMoviesDtoAsync(int? year = null, string? title = null, int? genreId = null,
            int? directorId = null, int? actorId = null)
        {
            var movies = await GetMoviesQuery(year, title, genreId, directorId, actorId, asNoTracking: true).ToListAsync();
            return movies.Select(movie => new DtoMovie(movie)).ToList();
        }
        public async Task<DtoMovie> GetMovieDtoAsync(int id)
        {
            var movie = await _db.Movie.FindAsync(id);
            return movie == default ? new DtoMovie() : new DtoMovie(movie);
        }

        public async Task<Movie> GetMovieAsync(int id)
        {
            var movie = await _db.Movie.FindAsync(id);

            return movie ?? throw new NotFoundException<Movie>();
        }

        public DtoMovie GetRandomDtoMovie(int? year = null, string? title = null, int? genreId = null, int? directorId = null,
            int? actorId = null)
        {
            var movie = GetMoviesQuery(year, title, genreId, directorId, actorId).GetRandom();
            return movie == default ? new DtoMovie() : new DtoMovie(movie);
        }

        public async Task<Movie> AddAsync(RequestMovie reqModel)
        {
            var newMovie = new Movie
            {
                Title = reqModel.Title,
                Description = reqModel.Description,
                ReleaseDate = reqModel.ReleaseDate,
            };

            await _db.Movie.AddAsync(newMovie);
            return await _db.SaveChangesAsync() != 0 ? newMovie : throw new AddingException<Movie>();
        }

        public async Task<Movie> EditAsync(RequestMovie reqModel, int id)
        {
            var movie = await GetMovieAsync(id);

            movie.Description = reqModel.Description;
            movie.Title = reqModel.Title;
            movie.ReleaseDate = reqModel.ReleaseDate;

            return await _db.SaveChangesAsync() != 0 ? movie : throw new EditingException<Movie>();
        }


        public async Task<bool> Delete(int id)
        {
            var movie = await GetMovieAsync(id);
            
            _db.Movie.Remove(movie);
            return await _db.SaveChangesAsync() != 0;
        }


        public async Task<List<Movie>> GetUserMoviesAsync(int id)
        {
            var query = GetMoviesQuery();
            return await query.Where(x => x.UsersFavorite.Any(user => user.Id == id)).ToListAsync();
        }

        public async Task<bool> AddUserMovieAsync(User user, int movieId)
        {
            if (user.UserFavouriteMovies.Any(x => x.Id == movieId))
                return false;

            var movie = await GetMovieAsync(movieId);
            user.UserFavouriteMovies.Add(movie);
            _db.User.Update(user);
            return await _db.SaveChangesAsync() != 0;
        }

        public async Task<bool> DeleteFromUserMovies(User user, int movieId)
        {

            if (user.UserFavouriteMovies.Any(x => x.Id != movieId))
                return false;

            var movie = await GetMovieAsync(movieId);

            user.UserFavouriteMovies.Remove(movie);
            _db.User.Update(user);
            return await _db.SaveChangesAsync() != 0;
        }


        public IQueryable<Movie> GetMoviesQuery(int? year = null, string? title = null, int? genreId = null, int? directorId = null,
            int? actorId = null, bool includeReviews = true, bool includeActors = false,bool includePoster = true,  bool asNoTracking = false)
        {
            var query = PrepareQuery(includeReviews, includeActors, includePoster, asNoTracking)!;

            if ((year ?? 0) != 0)
                query = query.Where(x => x.ReleaseDate.Year == year);
            if ((genreId ?? 0) != 0)
                query = query.Where(x => x.Genres.Any(x => x.Id == genreId));
            if ((directorId ?? 0) != 0)
                query = query.Where(x => x.Director != null && x.Director.Id == directorId);
            if ((actorId ?? 0) != 0)
                query = query.Where(x => x.Actors.Any(d => d.Id == actorId));

            if (title != null)
                query = query.Where(x => x.Title.Contains(title));

            return query;
        }
        private IQueryable<Movie> PrepareQuery(bool includePoster, bool includeReviews, bool includeActors, bool asNoTracking)
        {
            var query = _db.Movie
                .Include(x => x.Genres)
                .Include(x => x.Director)
                .AsQueryable();

            if (includePoster)
                query = query.Include(movie => movie.Posters);
            if (includeReviews) 
                query = query.Include(x => x.Reviews).ThenInclude(x => x.User);
            if (includeActors)
                query = query.Include(movie => movie.Actors);
            if (asNoTracking)
                query = query.AsNoTracking();

            return query;   
        }
    }
}