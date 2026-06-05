using Application.Interfaces;
using Domain;
using Domain.Classes;

namespace Persistence;

internal class ReviewRepository: IReviewRepository
{
    private readonly AppDbContext _context;

    public ReviewRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task AddReviewAsync(Review newReview)
    {
        throw new NotImplementedException();
    }
    
}