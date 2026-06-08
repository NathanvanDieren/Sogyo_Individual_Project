using Application.DTOs;

namespace Application.Interfaces;


public interface IUserFacade
{
    public Task<UserResponseDto> CreateUser(string username, string email, string password);
    public Task<LoginResponseDto?> UserLogin(string email, string hashedPassword);
    
}