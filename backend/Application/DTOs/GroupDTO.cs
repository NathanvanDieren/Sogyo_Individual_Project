namespace Application.DTOs;

public record CreateGroupDto(
    string Name,
    Guid CreatorId,
    List<string> Emails
);

public record GroupResponseDto(
    Guid GroupId
);

public class GroupMemberDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class GroupDto 
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<GroupMemberDto> Members { get; set; } = new(); 
}

public class GroupListDto 
{
    public required List<GroupDto> Groups { get; set; }
    public int TotalCount { get; set; }
}