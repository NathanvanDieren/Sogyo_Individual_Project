namespace Domain;
public interface IUserRepository
{
    Task AddUserAsync(User newUser);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByIdAsync(Guid id);
    
}