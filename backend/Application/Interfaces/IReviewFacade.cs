using Application.DTOs;
using Domain;

namespace Application.Interfaces;

public interface IReviewFacade
{
    public Task<ReviewResponseDto> CreateReview(CreateReviewDto model);
    public Task<IEnumerable<object>> GetItemTypes();
    public Task<ReviewListDto> GetReviews();
    public Task<ReviewListDto> GetReviewsByGroupId(Guid groupId);

    public Task DeleteReviewByReviewId(Guid userId);

    public Task<ReviewResponseDto> EditReview(Guid reviewId, CreateReviewDto model);
}
