namespace Api.DTOs;

public record CreateUserDto(
    string UserName, 
    string Email, 
    string Password
);

public record UserResponseDto(
    Guid Id,
    string Username,
    string Email, 
    string Role
);