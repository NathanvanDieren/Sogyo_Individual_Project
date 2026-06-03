namespace Domain;
public interface IUserRepository
{
    Task AddUserAsync(User newUser);
    Task<User?> GetUserByEmailAsync(string email);
    
    Task<List<User>> GetUsersByEmailsAsync(List<string> emails);
    Task<User?> GetUserByIdAsync(Guid id);
    
}