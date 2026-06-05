using Domain;
using Application.DTOs;
namespace Application.Interfaces;

public interface IGroupRepository
{
    public Task AddGroupAsync(Group newGroup);

    public Task<List<Group>> GetGroupsByGuidAsync(List<Guid> guids);

    public Task<GroupListDto> GetAllGroupsByUserIdAsync(Guid userId);
}