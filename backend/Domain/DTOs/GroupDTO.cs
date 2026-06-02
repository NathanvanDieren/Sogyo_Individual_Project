namespace Application.DTOs;

public record CreateGroupDto(
    string Name,
    Guid CreatorId
);