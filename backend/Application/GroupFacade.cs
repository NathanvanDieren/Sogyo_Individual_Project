using Application.DTOs;
using Application.Interfaces;
using Domain;

namespace Application; 

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

    public async Task<GroupResponseDto> CreateGroup(CreateGroupDto model)
    {
        var currentUser = _currentUserService.User;
        if (currentUser == null)
        {
            throw new UnauthorizedAccessException("Gebruiker is niet ingelogd.");
        }
        
        var newGroup = new Group(model.Name, currentUser);

        IEnumerable<User> targetMembers = Enumerable.Empty<User>();
        
        if (model.Emails != null && model.Emails.Any())
        {
            targetMembers = await _userRepository.GetUsersByEmailsAsync(model.Emails);
        }
        
        newGroup.UpdateMembers(targetMembers);
        
        await _groupRepository.AddGroupAsync(newGroup);
        
        return new GroupResponseDto(newGroup.Id);
    }
    
    public async Task<GroupResponseDto> EditGroup(Guid groupId, CreateGroupDto model)
    {
        var currentUser = _currentUserService.User;
        if (currentUser == null)
        {
            throw new UnauthorizedAccessException("Gebruiker is niet ingelogd.");
        }

        Group? group = await _groupRepository.GetGroupAndMembersByGroupIdAsync(groupId);
        if (group.CreatorId != currentUser.Id)
        {
            throw new UnauthorizedAccessException("Je mag geen reviews van andere gebruikers bewerken.");
        }
        group.ChangeName(model.Name);
        
        IEnumerable<User> targetMembers = Enumerable.Empty<User>();
        
        if (model.Emails != null && model.Emails.Any())
        {
            targetMembers = await _userRepository.GetUsersByEmailsAsync(model.Emails);
        }
        
        group.UpdateMembers(targetMembers);

        await _groupRepository.SaveChangesAsync();
        
        return new GroupResponseDto(group.Id);
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
        {    var currentUser = _currentUserService.User;
            Group? group = await _groupRepository.GetGroupByGroupIdAsync(groupId);
            if (group.Creator != currentUser)
            {
                throw new UnauthorizedAccessException("Je mag geen reviews van andere gebruikers bewerken.");
            }
            await _groupRepository.DeleteGroupAsync(groupId);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Kon de groep niet verwijderen.", ex);
        }
    }
    
}