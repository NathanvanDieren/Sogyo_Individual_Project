namespace Domain;

internal class Group
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    
    public Guid CreatorId { get; private set; }
    public User Creator { get; private set; } 

    private readonly List<User> _members = new();
    public IReadOnlyCollection<User> Members => _members.AsReadOnly();
    
    public Group(string name, User creator)
    {
        Id = Guid.NewGuid();
        Name = name;
        
        Creator = creator ?? throw new ArgumentNullException(nameof(creator));
        CreatorId = creator.Id;
        
        _members.Add(creator);
    }

    public void AddMember(User user)
    {
        if (user != null && !_members.Contains(user)) _members.Add(user);
    }
    
    public void RemoveMember(User user)
    {
        if (user != null && !_members.Contains(user)) _members.Remove(user);
    }

    protected Group() {} 
}