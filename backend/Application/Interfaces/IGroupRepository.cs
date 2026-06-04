using Domain;
using Application.DTOs;
namespace Application.Interfaces;

public interface IGroupRepository
{
    public Task AddGroupAsync(Group newGroup);

    public Task<GroupListDto> GetAllGroupsByUserIdAsync(Guid userId);
}