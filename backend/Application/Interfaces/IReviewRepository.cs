using Application.DTOs;
using Domain.Classes;

namespace Application.Interfaces;

public interface IReviewRepository
{
    Task AddReviewAsync(Review newReview);
}