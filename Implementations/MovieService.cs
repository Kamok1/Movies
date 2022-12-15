using Abstractions;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Models.Exceptions;
using Models.Movie;

namespace Implementations
{
    public class MovieService : IMovieService
    {
        private readonly AppDbContext _db;

        public MovieService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<DtoMovie>> GetMoviesDtoAsync(int year, string? title, int genreId, int directorId, int actorId, int page,
            int pageSize, string orderBy)
        {
            var movies = GetMoviesQuery(year, title, genreId, directorId, actorId, asNoTracking: true);
            if (orderBy.Contains("Rating"))
                movies = orderBy.Contains("-") ? movies.OrderByDescending(movie => movie.Reviews.Average(x => x.Rate))
                        : movies.OrderBy(movie => movie.Reviews.Average(x => x.Rate));

            ; return await movies.OrderByPropertyName(orderBy).Pagination(page, pageSize)
                            .Select(movie => new DtoMovie(movie)).ToListAsync();
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

        public async Task<DtoMovie> GetRandomDtoMovie(int? year = null, string? title = null, int? genreId = null, int? directorId = null,
            int? actorId = null)
        {
            var movie = await GetMoviesQuery(year, title, genreId, directorId, actorId).GetRandom();
            return movie == default ? new DtoMovie() : new DtoMovie(movie);
        }
        public async Task<List<DtoMovie>> GetUserMoviesAsync(int id)
        {
            var movies = GetMoviesQuery().Where(x => x.UsersFavorite.Any(user => user.Id == id));
            return await movies.Select(movie => new DtoMovie(movie)).ToListAsync();
        }

        public async Task AddAsync(RequestMovie reqModel)
        {
            var newMovie = new Movie
            {
                Title = reqModel.Title,
                Description = reqModel.Description,
                ReleaseDate = reqModel.ReleaseDate,
            };

            await _db.Movie.AddAsync(newMovie);
            if (await _db.SaveChangesAsync() == 0)
                throw new AddingException<Movie>();
        }
        public async Task EditAsync(RequestMovie reqModel, int id)
        {
            var movie = await GetMovieAsync(id);

            movie.Description = reqModel.Description;
            movie.Title = reqModel.Title;
            movie.ReleaseDate = reqModel.ReleaseDate;

            if (await _db.SaveChangesAsync() == 0)
                throw new EditingException<Movie>();
        }
        public async Task Delete(int id)
        {
            _db.Movie.Remove(await GetMovieAsync(id));
            if (await _db.SaveChangesAsync() == 0)
                throw new DeletingException<Movie>();
        }
        public async Task AddUserMovieAsync(User user, int movieId)
        {
            if (user.UserFavouriteMovies.Any(x => x.Id == movieId))
                return;

            var movie = await GetMovieAsync(movieId);
            user.UserFavouriteMovies.Add(movie);
            _db.User.Update(user);
            if (await _db.SaveChangesAsync() == 0)
                throw new AddingException<Movie>();
        }
        public async Task DeleteFromUserMovies(User user, int movieId)
        {

            if (user.UserFavouriteMovies.Any(x => x.Id != movieId))
                return;

            var movie = await GetMovieAsync(movieId);
            user.UserFavouriteMovies.Remove(movie);
            _db.User.Update(user);
            if (await _db.SaveChangesAsync() == 0)
                throw new DeletingException<Movie>();
        }
        private IQueryable<Movie> GetMoviesQuery(int? year = null, string? title = null, int? genreId = null, int? directorId = null,
            int? actorId = null, bool includeReviews = true, bool includeActors = false, bool includePoster = true, bool asNoTracking = false)
        {
            var query = PrepareQuery(includePoster, includeReviews, includeActors, asNoTracking);

            if (year.IsPositive())
                query = query.Where(movie => movie.ReleaseDate.Year == year);
            if (genreId.IsPositive())
                query = query.Where(movie => movie.Genres.Any(genre => genre.Id == genreId));
            if (directorId.IsPositive())
                query = query.Where(movie => movie.Director != null && movie.Director.Id == directorId);
            if (actorId.IsPositive())
                query = query.Where(movie => movie.Actors.Any(actor => actor.Id == actorId));
            if (string.IsNullOrEmpty(title))
                query = query.Where(movie => movie.Title.Contains(title!));

            return query;
        }
        private IQueryable<Movie> PrepareQuery(bool includePoster, bool includeReviews, bool includeActors, bool asNoTracking)
        {
            var query = _db.Movie
                .Include(movie => movie.Genres)
                .Include(movie => movie.Director)
                .AsQueryable();

            if (includePoster)
                query = query.Include(movie => movie.Posters);
            if (includeReviews)
                query = query.Include(movie => movie.Reviews).ThenInclude(review => review.User);
            if (includeActors)
                query = query.Include(movie => movie.Actors);
            if (asNoTracking)
                query = query.AsNoTracking();

            return query;
        }
    }
}