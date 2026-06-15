namespace Application.DTOs;

public record CreateUserDto(
    string Username,
    string Email,
    string Password
);

public record UserResponseDto(
    Guid Id,
    string Username,
    string Email,
    string Role
);
