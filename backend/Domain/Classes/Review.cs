namespace Domain.Classes;

public class Review
{
    public Guid Id { get; private set; }
    public User Creator { get; private set; }
    public string Title { get; private set; }
    public double Rating { get; private set; }
    public string Description { get; private set; }
    public ItemType ItemType { get; private set; }
    public DateTime LastUpdated { get; private set; }

    private readonly List<Group> _groups = new();

    public IReadOnlyCollection<Group> Groups => _groups.AsReadOnly();

    public Review(User creator, string title, double rating, string description, ItemType itemType, List<Group> groups)
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
    public Review(User creator, string title, double rating, string description, ItemType itemType)
        : this(creator, title, rating, description, itemType, new List<Group>())
    {
    }

    private void AddGroup(Group group)
    {
        if (group != null && !_groups.Contains(group))
        {
            _groups.Add(group);
            ChangeLastUpdated();
        }
    }

    private void RemoveGroup(Group group)
    {
        if (group != null && _groups.Contains(group))
        {
            _groups.Remove(group);
            ChangeLastUpdated();
        }
    }
    public void UpdateGroups(IEnumerable<Group> targetGroups)
    {
        var newGroupList = targetGroups ?? Enumerable.Empty<Group>();

        var groupsToRemove = _groups
            .Where(currentGroup => !newGroupList.Any(targetGroup => targetGroup.Id == currentGroup.Id))
            .ToList();

        foreach (var group in groupsToRemove)
        {
            RemoveGroup(group);
        }

        var groupsToAdd = newGroupList
            .Where(targetGroup => !_groups.Any(currentGroup => currentGroup.Id == targetGroup.Id))
            .ToList();

        foreach (var group in groupsToAdd)
        {
            AddGroup(group);
        }
    }

    public void ChangeTitle(string title)
    {
        Title = title;
        ChangeLastUpdated();
    }

    public void ChangeRating(double rating)
    {
        Rating = rating;
        ChangeLastUpdated();
    }

    public void ChangeDescription(string description)
    {
        Description = description;
        ChangeLastUpdated();
    }

    public void ChangeItemType(ItemType itemType)
    {
        ItemType = itemType;
        ChangeLastUpdated();
    }

    public void EnsureCanEdit(User user)
    {
        if (Creator != user)
        {
            throw new UnauthorizedAccessException("Je bent niet gemachtigd om deze review aan te passen.");
        }
    }

    private void ChangeLastUpdated()
    {
        LastUpdated = DateTime.UtcNow;
    }

    protected Review() { }
}
