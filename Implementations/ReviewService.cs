using Abstractions;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Models.Exceptions;
using Models.Review;

namespace Implementations;

public class ReviewService : IReviewService
{
    private readonly AppDbContext _db;

    public ReviewService(AppDbContext db)
    {
        _db = db;
    }
    public async Task AddAsync(RequestReview reqModel, Movie movie, User user)
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
        if (await _db.SaveChangesAsync() == 0)
            throw new AddingException<Review>();
    }

    public async Task<int> CountMovieReviews(int movieId)
    {
        return await _db.Review.Where(review => review.Movie.Id == movieId).CountAsync();
    }

    public async Task<List<DtoReview>> GetMovieReviewsAsync(int movieId, int page, int pageSize, string orderBy)
    {
        var reviews = _db.Review.Where(x => x.Movie.Id == movieId)
            .Include(x => x.User)
            .AsNoTracking();

        return await reviews.OrderByPropertyName(orderBy).Pagination(page, pageSize)
            .Select(review => new DtoReview(review)).ToListAsync();
    }
    public async Task EditAsync(RequestReview reqReview, int reviewId, User user)
    {
        var review = await _db.Review.Where(x => x.Id == reviewId).Include(x => x.User).FirstOrDefaultAsync();
        if (review == null || review.User != user)
            throw new NotFoundException<Review>();

        review.Rate = reqReview.Rate;
        review.Body = reqReview.Body;
        review.Title = reqReview.Title;
        _db.Review.Update(review);
        if (await _db.SaveChangesAsync() == 0)
            throw new EditingException<Review>();
    }

    public async Task DeleteAsync(int reviewId)
    {
        var review = await _db.Review.FindAsync(reviewId);
        if (review != null) _db.Review.Remove(review);
        if (await _db.SaveChangesAsync() == 0)
            throw new DeletingException<Review>();
    }
}