using Application.DTOs;
using Application.Interfaces;
using Domain;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;

public class GroupFacade: IGroupFacade
{   
    private readonly IGroupRepository _groupRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public GroupFacade(IGroupRepository groupRepository, IUserRepository userRepository, ICurrentUserService currentUserService)
    {
        _groupRepository = groupRepository;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<GroupResponseDto> CreateGroup(string name, List<string> emails)
    {
        var currentUser = _currentUserService.User;
        if (currentUser == null)
        {
            throw new UnauthorizedAccessException("Gebruiker is niet ingelogd.");
        }
        
        var newGroup = new Group(name, currentUser);

        if (emails != null && emails.Any())
        {
            var existingUsers = await _userRepository.GetUsersByEmailsAsync(emails);
            
            foreach (var user in existingUsers)
            {
                newGroup.AddMember(user);
            }
        }
        
        await _groupRepository.AddGroupAsync(newGroup);

        // 6. Return de DTO
        return new GroupResponseDto(newGroup.Id);
    }
    
    public async Task<GroupListDto> GetGroups()
    {
        var currentUser = _currentUserService.User;
        if (currentUser == null)
        {
            throw new UnauthorizedAccessException("Gebruiker is niet ingelogd.");
        }
        
        var groups = await _groupRepository.GetAllGroupsByUserIdAsync(currentUser.Id);

        return groups;
    }
    
}