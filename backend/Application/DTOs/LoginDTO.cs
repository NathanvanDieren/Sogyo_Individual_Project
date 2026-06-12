namespace Application.DTOs;

public record LoginResponseDto(
    Guid Id,
    string Token
);

public record LoginDto
(
    string Email,
    string Password
);

public record ValidateResponseDto
(
    Guid UserId
);
