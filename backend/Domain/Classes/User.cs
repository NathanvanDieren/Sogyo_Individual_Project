namespace Domain;

public class User
{
    public Guid Id { get; private set; } 
    
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string RoleId { get; private set; } 
    
    public DateTime LastUpdated { get; private set; }

    public User(string username, string email, string passwordHash, string roleId)
    {
        Id = Guid.NewGuid();
        Username = username;
        Email = email.ToLower().Trim();
        PasswordHash = passwordHash;
        RoleId = roleId;
        ChangeLastUpdated();
    }
    public bool ChangeUsername(string newUsername)
    {
        if (Username.ToLower().Trim() == newUsername.ToLower().Trim())
        {
            return false; 
        }
        
        Username = newUsername.Trim();
        ChangeLastUpdated();
        return true;
    }

    private void ChangeLastUpdated()
    {
        LastUpdated = DateTime.Now;
    }

    protected User() { } 
}