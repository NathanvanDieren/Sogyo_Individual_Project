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
        
        return new GroupResponseDto(newGroup.Id);
    }
    
    public async Task<ReviewResponseDto> EditGroup(Guid groupId, CreateGroupDto model)
    {
        var currentUser = _currentUserService.User;
        if (currentUser == null)
        {
            throw new UnauthorizedAccessException("Gebruiker is niet ingelogd.");
        }

        Group? group = await _groupRepository.GetGroupAndMembersByGroupIdAsync(groupId);
        group.ChangeName(model.Name);
        
        IEnumerable<User> targetMembers = Enumerable.Empty<User>();
        
        if (model.Emails != null && model.Emails.Any())
        {
            targetMembers = await _userRepository.GetUsersByEmailsAsync(model.Emails);
        }
        
        group.UpdateMembers(targetMembers);

        await _groupRepository.SaveChangesAsync();
        
        return new ReviewResponseDto(group.Id);
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

    public async Task DeleteGroupByGroupId(Guid groupId)
    {
        try
        {
            await _groupRepository.DeleteGroupAsync(groupId);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Kon de groep niet verwijderen.", ex);
        }
    }
    
}