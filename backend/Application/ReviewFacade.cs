using System.Net;
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

        if (model.GroupsGuids != null && model.GroupsGuids.Any())
        {
            var existingUsers = await _groupRepository.GetGroupsByGuidAsync(model.GroupsGuids);
            
            foreach (var user in existingUsers)
            {
                newReview.AddGroup(user);
            }
        }
        
        await _reviewRepository.AddReviewAsync(newReview);
        
        return new ReviewResponseDto(newReview.Id);
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
        
        var reviews = await _reviewRepository.GetAllReviewsByUserIdAsync(currentUser);

        return reviews;
    }
}