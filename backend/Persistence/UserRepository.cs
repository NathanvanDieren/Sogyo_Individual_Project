using Domain;
using Domain.DTOs;
using Microsoft.EntityFrameworkCore;

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
}