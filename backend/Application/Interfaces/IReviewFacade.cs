using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IReviewFacade
{
    public Task<ReviewResponseDto> CreateReview(CreateReviewDto model);
    public Task<IEnumerable<object>> GetItemTypes();
    public Task<ReviewListDto> GetReviews();
    public Task<ReviewListDto> GetReviewsByGroupId(Guid groupId);
}