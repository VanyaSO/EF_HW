using HW_8.Models;

namespace HW_8.Interfaces;

public interface IReview
{
    Task<IEnumerable<Review>> GetAllReviewsAsync(int bookId);
    Task<Review> GetReviewAsync(int id);

    Task AddReviewAsync(Review review);
    Task DeleteReviewAsync(Review review);
}