namespace Domain;

public class User
{
    public string PasswordHash { get; private set; }
    
    public string Name { get; private set; }
    public string Email { get; private set; }
    
    public string RoleId {get, private set; }

    public User(string name, string email, string passwordHash)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
    }
    
}
