using Domain;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
namespace Persistence;

internal class UserRepository: IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddUserAsync(User newUser)
    {
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetUserByIdAsync(Guid guid)
    {
        User? user = await _context.Users
            .SingleOrDefaultAsync(u => u.Id == guid);
        return user;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        User? user = await _context.Users
            .SingleOrDefaultAsync(u => u.Email == email);
        return user;
    }

    public async Task<List<User>> GetUsersByEmailsAsync(List<string> emails)
    {
        if (emails == null || !emails.Any())
        {
            return new List<User>();
        }

        return await _context.Users
            .Where(u => emails.Contains(u.Email))
            .ToListAsync();
    }

}