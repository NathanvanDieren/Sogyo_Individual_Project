namespace Domain.DTOs;
public record LoginResponseDto(
    Guid Id,
    string Token
);

public record LoginDto
(
    string Email,
    string Password
);