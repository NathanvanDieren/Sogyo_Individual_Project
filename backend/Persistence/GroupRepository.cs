using Domain;
using Application.Interfaces;
using Application.DTOs;
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
    
    public async Task<List<Group>> GetGroupsByGuidAsync(List<Guid> guids)
    {
        if (guids == null || !guids.Any())
        {
            return new List<Group>();
        }

        return await _context.Groups
            .Where(u => guids.Contains(u.Id))
            .ToListAsync();
    }

    public async Task<Group?> GetGroupAndMembersByGroupIdAsync(Guid groupId)
    {
        return await _context.Groups
            .Include(r => r.Members) 
            .FirstOrDefaultAsync(r => r.Id == groupId);
    }
    public async Task<GroupListDto> GetAllGroupsByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return new GroupListDto { Groups = new List<GroupDto>(), TotalCount = 0 };
        }

        var groups = await _context.Groups
            .Where(g => g.CreatorId == userId || g.Members.Any(m => m.Id == userId))
            .Select(g => new GroupDto
            {
                Id = g.Id,
                Name = g.Name,
                Members = g.Members.Select(m => new GroupMemberDto
                {
                    Id = m.Id,
                    Name = m.Username,
                    Email = m.Email
                }).ToList()
            })
            .ToListAsync();

        return new GroupListDto
        {
            Groups = groups,
            TotalCount = groups.Count
        };
    }

    public async Task DeleteGroupAsync(Guid groupId)
    {
        var group = await _context.Groups.FindAsync(groupId);
        
        if (group != null)
        {
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
        }
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}