using Domain.DTOs;

namespace Domain;


public interface IUserFacade
{
    public Task<Guid?> CreateUser(string username, string email, string password);
    
    public Task ChangePassword(User user, string password);
}