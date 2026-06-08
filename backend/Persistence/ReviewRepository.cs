using Application.DTOs;
using Application.Interfaces;
using Domain;
using Domain.Classes;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

internal class ReviewRepository: IReviewRepository
{
    private readonly AppDbContext _context;

    public ReviewRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddReviewAsync(Review newReview)
    {
        _context.Reviews.Add(newReview);
        await _context.SaveChangesAsync();
    }
    
    public async Task<ReviewListDto> GetAllReviewsByUserIdAsync(User user)
    {
        if (user == null)
        {
            return new ReviewListDto { Reviews = new List<ReviewDto>(), TotalCount = 0 };
        }

        var reviews = await _context.Reviews
            .Where(g => g.Creator == user)
            .Select(g => new ReviewDto
            {
                Id = g.Id,
                Title = g.Title,
                Rating = g.Rating,
                Description = g.Description,
                ItemType =  g.ItemType.ToString(),
                Groups = g.Groups.Select(m => new GroupInReviewDto
                {
                    Id = m.Id,
                    Name = m.Name
                }).ToList()
            })
            .ToListAsync();

        return new ReviewListDto()
        {
            Reviews = reviews,
            TotalCount = reviews.Count
        };
    }
}