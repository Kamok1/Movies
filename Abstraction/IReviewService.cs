using Data.Models;
using Models.Review;

namespace Abstractions;

public interface IReviewService
{
    Task AddAsync(RequestReview review, Movie movie, User user);
    Task<int> CountMovieReviews(int movieId);
    Task<List<DtoReview>> GetMovieReviewsAsync(int movieId, int page, int pageSize, string orderBy);
    Task EditAsync(RequestReview review, int reviewId, User user);
    Task DeleteAsync(int reviewId);
}