using Application.DTOs;
using Domain;
using Domain.Classes;

namespace Application.Interfaces;

public interface IReviewRepository
{
    Task AddReviewAsync(Review newReview);
    
    Task SaveChangesAsync(); 
    Task<Review?> GetReviewByReviewIdAsync(Guid reviewId);

    Task<ReviewListDto> GetAllReviewsByUserIdAsync(Guid userId);
    
    Task<Review> GetReviewWithGroupsByReviewIdAsync(Guid reviewId);
    public Task<ReviewListDto> GetReviewsByGroupIdAsync(User currentUser, Guid groupId);
    
    Task DeleteReviewAsync(Guid reviewId);
}