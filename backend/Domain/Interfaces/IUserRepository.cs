namespace Domain;
internal interface IUserRepository
{
    Task AddUserAsync(User newUser);
    Task<User?> GetUserByEmail(string email);
    Task<User?> GetUserById(Guid id);
    
}