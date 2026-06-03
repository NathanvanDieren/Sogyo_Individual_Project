using Domain.Services;
using Domain.DTOs;
using Microsoft.AspNetCore.Http;
using BC = BCrypt.Net.BCrypt;

namespace Domain;

internal class UserFacade : IUserFacade
{
    private readonly IUserRepository _userRepository;
    private readonly TokenService _tokenService;
    
    public UserFacade(IUserRepository userRepository, TokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<UserResponseDto> CreateUser(string username, string email, string password)
    {
        var existingUser = await _userRepository.GetUserByEmailAsync(email);
        if (existingUser != null)
        {
            throw new BadHttpRequestException("Dit e-mailadres is al in gebruik.");
        }
        
        string hashedPassword = BC.HashPassword(password);
        
        User newUser = new User(username, email, hashedPassword, "user");
        
        await _userRepository.AddUserAsync(newUser);
        
        return new UserResponseDto(
            newUser.Id,
            newUser.Username,
            newUser.Email,
            newUser.RoleId
        );
    }

    public async Task<UserResponseDto?> GetUserById(Guid guid)
    {
        User? user = await _userRepository.GetUserByIdAsync(guid);
        
        if (user == null) return null;
        
        return new UserResponseDto(
            user.Id,
            user.Username,
            user.Email,
            user.RoleId
        );
    }
    
    public async Task<LoginResponseDto?> UserLogin(string email, string password)
    {
        User? user = await _userRepository.GetUserByEmailAsync(email);
        
        if (user == null) return null;
        
        bool isPasswordCorrect = BC.Verify(password, user.PasswordHash);
        
        if (!isPasswordCorrect) return null;

        string token = _tokenService.GenerateToken(user);
            
        return new LoginResponseDto(
            user.Id,
            token 
        );
    }
    
}