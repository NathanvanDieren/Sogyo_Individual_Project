namespace Domain.Classes;

public class Review
{
    public Guid Id { get; private set; }
    public User Creator { get; private set; }
    public string Title { get; private set; }
    public int Rating { get; private set; }
    public string Description { get; private set; }
    public ItemType ItemType { get; private set; }
    public DateTime LastUpdated { get; private set; }
    
    private readonly List<Group> _groups = new();
    
    public IReadOnlyCollection<Group> Groups => _groups.AsReadOnly();
    
    public Review(User creator, string title, int rating, string description, ItemType itemType, List<Group> groups)
    {
        Id = Guid.NewGuid();
        Creator = creator;
        Title = title;
        Rating = rating;
        Description = description;
        ItemType = itemType;
        LastUpdated = DateTime.UtcNow;
        
        if (groups != null)
        {
            _groups.AddRange(groups);
        }
    }
    public Review(User creator, string title, int rating, string description, ItemType itemType) 
        : this(creator, title, rating, description, itemType, new List<Group>())
    {
    }
    
    public void AddGroup(Group group)
    {
        if (group != null && !_groups.Contains(group)) 
        {
            _groups.Add(group);
            LastUpdated = DateTime.UtcNow;
        }
    }
    
    public void RemoveGroup(Group group)
    {
        if (group != null && _groups.Contains(group)) 
        {
            _groups.Remove(group);
            LastUpdated = DateTime.UtcNow;
        }
    }

    protected Review() {}
}