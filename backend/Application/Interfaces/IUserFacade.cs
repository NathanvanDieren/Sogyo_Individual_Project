using System.ComponentModel.DataAnnotations;
using Application.DTOs;

namespace Application.Interfaces;


public interface IUserFacade
{
    public Task<UserResponseDto> CreateUser(string username, string email, string password);
    public Task<UserResponseDto?> GetUserById(Guid guid);
    
    public Task<LoginResponseDto?> UserLogin(string email, string hashedPassword);
    
}