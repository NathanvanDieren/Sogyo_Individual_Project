using Application.DTOs;
using Application.Interfaces;
using Domain;
using Domain.Classes;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

internal class ReviewRepository : IReviewRepository
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
    
    public async Task<Review?> GetReviewWithGroupsByReviewIdAsync(Guid reviewId)
    {
        return await _context.Reviews
            .Include(r => r.Groups) 
            .FirstOrDefaultAsync(r => r.Id == reviewId);
    }

    public async Task<ReviewListDto> GetAllReviewsByUserIdAsync(User user)
    {
        if (user != null)
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
                Itemtype = g.ItemType.ToString(),
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

    public async Task<ReviewListDto> GetReviewsByGroupIdAsync(Guid groupId)
    {
        if (groupId == null)
        {
            return new ReviewListDto { Reviews = new List<ReviewDto>(), TotalCount = 0 };
        }

        var reviews = await _context.Reviews
            .Where(r => r.Groups.Any(g => g.Id == groupId))
            .Select(g => new ReviewDto
            {
                Id = g.Id,
                Title = g.Title,
                Rating = g.Rating,
                Description = g.Description,
                Itemtype = g.ItemType.ToString(),
                Name = g.Creator.Username
            })
            .ToListAsync();

        return new ReviewListDto()
        {
            Reviews = reviews,
            TotalCount = reviews.Count
        };
    }

    public async Task DeleteReviewAsync(Guid reviewId)
    {
        var review = await _context.Reviews.FindAsync(reviewId);

        if (review != null)
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }
    }

    public Task SaveChangesAsync()
    {
        return  _context.SaveChangesAsync();
    }

}