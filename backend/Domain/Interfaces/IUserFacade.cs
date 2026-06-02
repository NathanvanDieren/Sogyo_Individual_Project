using System.ComponentModel.DataAnnotations;
using Domain.DTOs;

namespace Domain;


public interface IUserFacade
{
    public Task<UserResponseDto> CreateUser(string username, string email, string password);
    public Task<UserResponseDto?> GetUserById(Guid guid);
    
    public Task<LoginResponseDto?> UserLogin(string email, string hashedPassword);
    
}