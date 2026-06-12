using Application.DTOs;
using Application.Interfaces;
using Domain.Classes;

namespace Application;

public class GroupFacade : IGroupFacade
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

        if (model.Emails.Any())
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

        var group = await _groupRepository.GetGroupAndMembersByGroupIdAsync(groupId);
        if (group == null)
        {
            throw new KeyNotFoundException("De opgevraagde groep kon niet worden gevonden.");
        }

        group.EnsureCanEdit(currentUser);

        group.ChangeName(model.Name);

        IEnumerable<User> targetMembers = Enumerable.Empty<User>();

        if (model.Emails.Any())
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
        var currentUser = _currentUserService.User;
        var group = await _groupRepository.GetGroupByGroupIdAsync(groupId);

        if (group == null)
        {
            throw new KeyNotFoundException("De opgevraagde groep kon niet worden gevonden.");
        }

        if (currentUser == null)
        {
            throw new KeyNotFoundException("U bent niet ingelogd.");
        }

        group.EnsureCanEdit(currentUser);

        await _groupRepository.DeleteGroupAsync(groupId);
    }

}
