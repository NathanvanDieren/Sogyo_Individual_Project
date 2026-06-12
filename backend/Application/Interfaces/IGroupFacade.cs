using Application.DTOs;

namespace Application.Interfaces;

public interface IGroupFacade
{
    public Task<GroupResponseDto> CreateGroup(CreateGroupDto model);

    public Task<GroupResponseDto> EditGroup(Guid groupId, CreateGroupDto model);

    public Task<GroupListDto> GetGroups();

    public Task DeleteGroupByGroupId(Guid groupId);

}
