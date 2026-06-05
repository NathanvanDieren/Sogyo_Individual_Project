using Domain;
namespace Application.DTOs;

public record CreateReviewDto(
    string Name,
    string Title,
    int Rating,
    string Description,
    ItemType ItemType,
    List<Guid> GroupsGuids
);

public record ReviewResponseDto(
    Guid Id
);

