using Domain;
namespace Application.DTOs;

public record CreateReviewDto(
    string Title,
    double Rating,
    string Description,
    ItemType ItemType,
    List<Guid> GroupsGuids
);

public record ReviewResponseDto(
    Guid GroupId
);

public class GroupInReviewDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class ReviewDto 
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public double Rating { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Itemtype { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool isCreator { get; set; } = false;
    public List<GroupInReviewDto> Groups { get; set; } = new(); 
    

}

public class ReviewListDto 
{
    public required List<ReviewDto> Reviews { get; set; }
    public int TotalCount { get; set; }
}
