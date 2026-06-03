using Domain;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

internal class GroupRepository: IGroupRepository
{
    private readonly AppDbContext _context;

    public GroupRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddGroupAsync(Group newGroup)
    {
        _context.Groups.Add(newGroup);
        await _context.SaveChangesAsync();
    }
    
    
}