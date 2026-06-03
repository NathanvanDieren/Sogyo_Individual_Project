namespace Application.DTOs;

public record CreateGroupDto(
    string Name,
    Guid CreatorId,
    List<string> Emails
);

public record GroupResponseDto(
    Guid GroupId
);