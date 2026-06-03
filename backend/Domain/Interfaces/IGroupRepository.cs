namespace Domain.Interfaces;

public interface IGroupRepository
{
    public Task AddGroupAsync(Group newGroup);
}