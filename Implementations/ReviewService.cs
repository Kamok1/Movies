using Abstractions;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Models.Review;
using Models.Settings;

namespace Implementations;

public class ReviewService : IReviewService
{
    private readonly AppDbContext _db;
    private readonly AppSettings _settings;

    public ReviewService(AppDbContext db, AppSettings settings)
    {
        _db = db;
        _settings = settings;
    }
    public async Task<Review?> AddAsync(RequestReview reqModel, Movie movie, User user)
    {
        var review = new Review
        {
            User = user,
            Movie = movie,
            Body = reqModel.Body,
            Created = DateTime.UtcNow,
            Rate = reqModel.Rate,
            Title = reqModel.Title
        };
        movie.Reviews.Add(review);
        return await _db.SaveChangesAsync() != 0 ? review : null;
    }

    public async Task<List<DtoReview>> GetMovieReviewsAsync(int movieId, int page, int pageSize)
    {
        var reviews = _db.Review.Where(x => x.Movie.Id == movieId)
            .Include(x => x.User)
            .AsNoTracking();

        if (page <= 0)
            return await reviews.Select(review => new DtoReview(review)).ToListAsync();
        if (pageSize <= 0)
            pageSize = _settings.PageSize;

        return await reviews.Skip(pageSize * --page)
            .Take(pageSize)
            .Select(review => new DtoReview(review)).ToListAsync();
    }

    public async Task<int> CountMovieReviewsAsync(int movieId)
    {
        return await _db.Review.Where(x => x.Movie.Id == movieId).CountAsync();
    }

    public async Task<bool> EditAsync(RequestReview reqReview, int reviewId, User user)
    {
        var review = await _db.Review.Where(x => x.Id == reviewId).Include(x=>x.User).FirstOrDefaultAsync();
        if (review == null || review.User != user)
            return false;

        review.Rate = reqReview.Rate;
        review.Body = reqReview.Body;
        review.Title = reqReview.Title;
        _db.Review.Update(review);
        return await _db.SaveChangesAsync() != 0;
    }

    public async Task<bool> DeleteAsync(int reviewId)
    {
        var review = await _db.Review.FindAsync(reviewId);
        if (review != null) _db.Review.Remove(review);
        return await _db.SaveChangesAsync() != 0;
    }
}