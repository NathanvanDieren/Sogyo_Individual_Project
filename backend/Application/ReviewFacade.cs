using Application.DTOs;
using Application.Interfaces;
using Domain;
using Domain.Classes;

namespace Application;

internal class ReviewFacade : IReviewFacade
{
    private readonly IReviewRepository _reviewRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IGroupRepository _groupRepository;

    public ReviewFacade(IReviewRepository reviewRepository, ICurrentUserService currentUserService, IGroupRepository groupRepository)
    {
        _reviewRepository = reviewRepository;
        _currentUserService = currentUserService;
        _groupRepository = groupRepository;
    }

    public async Task<ReviewResponseDto> CreateReview(CreateReviewDto model)
    {
        var currentUser = _currentUserService.User;
        if (currentUser == null)
        {
            throw new UnauthorizedAccessException("Gebruiker is niet ingelogd.");
        }

        var newReview = new Review(currentUser, model.Title, model.Rating, model.Description, model.ItemType);

        var targetGroups = Enumerable.Empty<Group>();

        if (model.GroupsGuids.Any())
        {
            targetGroups = await _groupRepository.GetGroupsByGuidAsync(model.GroupsGuids);
        }

        newReview.UpdateGroups(targetGroups);

        await _reviewRepository.AddReviewAsync(newReview);

        return new ReviewResponseDto(newReview.Id);
    }

    public async Task<ReviewResponseDto> EditReview(Guid reviewId, CreateReviewDto model)
    {
        var currentUser = _currentUserService.User;
        if (currentUser == null)
        {
            throw new UnauthorizedAccessException("Gebruiker is niet ingelogd.");
        }

        var review = await _reviewRepository.GetReviewWithGroupsByReviewIdAsync(reviewId);
        review.EditReview(currentUser, model.Title, model.Rating, model.Description, model.ItemType);

        var targetGroups = Enumerable.Empty<Group>();

        if (model.GroupsGuids.Any())
        {
            targetGroups = await _groupRepository.GetGroupsByGuidAsync(model.GroupsGuids);
        }

        review.UpdateGroups(targetGroups);

        await _reviewRepository.SaveChangesAsync();

        return new ReviewResponseDto(review.Id);
    }

    public async Task<IEnumerable<object>> GetItemTypes()
    {
        var itemTypes = Enum.GetValues(typeof(ItemType))
            .Cast<ItemType>()
            .Select(e => new
            {
                Value = (int)e,
                Name = e.ToString()
            });

        return await Task.FromResult(itemTypes);
    }

    public async Task<ReviewListDto> GetReviews()
    {
        var currentUser = _currentUserService.User;
        if (currentUser == null)
        {
            throw new UnauthorizedAccessException("Gebruiker is niet ingelogd.");
        }

        var reviews = await _reviewRepository.GetAllReviewsByUserIdAsync(currentUser.Id);

        return reviews;
    }

    public async Task<ReviewListDto> GetReviewsByGroupId(Guid groupId)
    {
        var currentUser = _currentUserService.User;
        if (currentUser == null)
        {
            throw new UnauthorizedAccessException("Gebruiker is niet ingelogd.");
        }

        var reviews = await _reviewRepository.GetReviewsByGroupIdAsync(currentUser, groupId);

        return reviews;
    }

    public async Task DeleteReviewByReviewId(Guid reviewId)
    {
        var currentUser = _currentUserService.User;
        var review = await _reviewRepository.GetReviewByReviewIdAsync(reviewId);

        if (review == null)
        {
            throw new KeyNotFoundException("De opgevraagde review kon niet worden gevonden.");
        }
        if (currentUser == null)
        {
            throw new UnauthorizedAccessException("U bent niet ingelogd.");
        }
        review.EnsureCanEdit(currentUser);

        await _reviewRepository.DeleteReviewAsync(reviewId);
    }
}
