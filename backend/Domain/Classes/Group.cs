namespace Domain;

public class Group
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    
    public Guid CreatorId { get; private set; }
    public User Creator { get; private set; } 
    
    public DateTime LastUpdated { get; private set; }

    private readonly List<User> _members = new();
    public IReadOnlyCollection<User> Members => _members.AsReadOnly();
    
    public Group(string name, User creator)
    {
        Id = Guid.NewGuid();
        Name = name;
        Creator = creator ?? throw new ArgumentNullException(nameof(creator));
        CreatorId = creator.Id;
        
        _members.Add(creator);
        LastUpdated = DateTime.UtcNow;
    }

    public void AddMember(User user)
    {
        if (user != null && !_members.Contains(user)) _members.Add(user);
    }
    
    public void RemoveMember(User user)
    {
        if (user != null && _members.Contains(user)) _members.Remove(user);
    }

    public void UpdateMembers(IEnumerable<User> targetMembers)
    {
        var newMemberList = targetMembers ?? Enumerable.Empty<User>();

        var memberToRemove = _members
            .Where(currentGroup => !newMemberList.Any(targetGroup => targetGroup.Id == currentGroup.Id))
            .ToList();

        foreach (var member in memberToRemove)
        {
            RemoveMember(member);
        }

        var memberToAdd = newMemberList
            .Where(targetGroup => !_members.Any(currentGroup => currentGroup.Id == targetGroup.Id))
            .ToList();

        foreach (var member in memberToAdd)
        {
            AddMember(member);
        }

        ChangeLastUpdated();
    }

    public void ChangeName(string newName)
    {
        Name = newName;
        ChangeLastUpdated();
    }
    public void ChangeLastUpdated()
    {
        LastUpdated = DateTime.UtcNow;
    }
    protected Group() {} 
}