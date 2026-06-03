using Application.DTOs;

namespace Application.Interfaces;

public interface IGroupFacade
{
    public Task<GroupResponseDto> CreateGroup(string name, List<string> emails);

}