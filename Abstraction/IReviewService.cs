using Data.Models;
using Models.Review;

namespace Abstractions;

public interface IReviewService
{
    Task<Review?> AddAsync(RequestReview review, Movie movie, User user);
    Task<List<DtoReview>> GetMovieReviewsAsync(int movieId, int page, int pageSize);
    Task<int> CountMovieReviewsAsync(int movieId);
    Task<bool> EditAsync(RequestReview review, int reviewId, User user);
    Task<bool> DeleteAsync(int reviewId);
}