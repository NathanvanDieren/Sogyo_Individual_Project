namespace Domain;

public class User
{
    public Guid Id { get; private set; } 
    
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    
    public string RoleId { get; private set; } 

    public User(string name, string email, string passwordHash, string roleId)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        RoleId = roleId;
    }

    protected User() { } 
}