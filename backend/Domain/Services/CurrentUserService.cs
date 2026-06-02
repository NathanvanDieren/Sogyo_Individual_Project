namespace Domain.Services;

public class CurrentUserService
{
    public User? User { get; internal set; }
    
    public bool IsAuthenticated => User != null;
    
    public void SetCurrentUser(User user)
    {
        if (User != null) throw new InvalidOperationException("User is al ingesteld voor dit request.");
        
        User = user ?? throw new ArgumentNullException(nameof(user));
    }
}