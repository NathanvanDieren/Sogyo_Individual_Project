using Application.DTOs;
using Domain;
using Domain.Classes;

namespace Application.Interfaces;

public interface IReviewRepository
{
    Task AddReviewAsync(Review newReview);

    Task<ReviewListDto> GetAllReviewsByUserIdAsync(User user);
}