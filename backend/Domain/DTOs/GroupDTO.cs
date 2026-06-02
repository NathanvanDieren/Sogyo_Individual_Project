namespace Domain.DTOs;

public record CreateGroupDto(
    string Name,
    Guid CreatorId
);