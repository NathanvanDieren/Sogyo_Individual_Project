using Application.DTOs;
using Domain;
using Domain.Classes;

namespace Application.Interfaces;

public interface IReviewRepository
{
    Task AddReviewAsync(Review newReview);
    
    Task SaveChangesAsync(); 

    Task<ReviewListDto> GetAllReviewsByUserIdAsync(Guid userId);
    
    Task<Review> GetReviewWithGroupsByReviewIdAsync(Guid reviewId);
    Task<ReviewListDto> GetReviewsByGroupIdAsync(Guid groupId);
    
    Task DeleteReviewAsync(Guid reviewId);
}