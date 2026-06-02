namespace Domain.DTOs;

public record CreateReviewDto(
    string Name,
    Guid CreatorId
);

public record ReviewResponseDto(
    Guid Id
);