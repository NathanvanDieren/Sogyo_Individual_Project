using Domain.Classes;
using Xunit;

namespace Domain.Tests;

public class ReviewTests
{
    private User _newUser1 = new User("Nathan", "nvdieren@sogyo.nl", "nvdieren", "user");

    [Fact]
    public void CreateReview_WithValidData_ShouldInitializeCorrectly()
    {
        var group1 = new Group("Boekenclub", _newUser1);
        var groups = new List<Group> { group1 };

        var title = "Het Gouden Ei";
        var description = "Ik vond hem erg spannend en vlot geschreven.";
        var rating = 5;
        var type = ItemType.Book;

        var review = new Review(_newUser1, title, rating, description, type, groups);

        Assert.NotNull(review);
    }

    [Fact]
    public void TestReviewIdIsNotEmpty()
    {
        var review = new Review(_newUser1, "Title", 5, "Description", ItemType.Book);
        Assert.NotEqual(Guid.Empty, review.Id);
    }

    [Fact]
    public void TestReviewTitleIsSetCorrectly()
    {
        var review = new Review(_newUser1, "MyTitle", 5, "Description", ItemType.Book);
        Assert.Equal("MyTitle", review.Title);
    }

    [Fact]
    public void TestReviewRatingIsSetCorrectly()
    {
        var review = new Review(_newUser1, "Title", 4.5, "Description", ItemType.Book);
        Assert.Equal(4.5, review.Rating);
    }

    [Fact]
    public void TestReviewDescriptionIsSetCorrectly()
    {
        var review = new Review(_newUser1, "Title", 5, "MyDescription", ItemType.Book);
        Assert.Equal("MyDescription", review.Description);
    }

    [Fact]
    public void TestReviewItemTypeIsSetCorrectly()
    {
        var review = new Review(_newUser1, "Title", 5, "Description", ItemType.Film);
        Assert.Equal(ItemType.Film, review.ItemType);
    }

    [Fact]
    public void TestReviewCreatorIsSetCorrectly()
    {
        var review = new Review(_newUser1, "Title", 5, "Description", ItemType.Book);
        Assert.Equal(_newUser1, review.Creator);
    }

    [Fact]
    public void TestReviewWithNullGroupsDoesNotThrow()
    {
        var review = new Review(_newUser1, "Title", 5, "Description", ItemType.Book, null!);
        Assert.NotNull(review);
    }

    [Fact]
    public void TestReviewWithEmptyGroupsList()
    {
        var review = new Review(_newUser1, "Title", 5, "Description", ItemType.Book, new List<Group>());
        Assert.Empty(review.Groups);
    }

    [Fact]
    public void TestReviewWithoutGroupsConstructor()
    {
        var review = new Review(_newUser1, "Title", 5, "Description", ItemType.Book);
        Assert.Empty(review.Groups);
    }

    [Fact]
    public void TestReviewContainsGroupsFromConstructor()
    {
        var group1 = new Group("Group1", _newUser1);
        var groups = new List<Group> { group1 };
        var review = new Review(_newUser1, "Title", 5, "Description", ItemType.Book, groups);
        Assert.Contains(group1, review.Groups);
    }

    [Fact]
    public void TestReviewGroupsCountFromConstructor()
    {
        var group1 = new Group("Group1", _newUser1);
        var group2 = new Group("Group2", _newUser1);
        var groups = new List<Group> { group1, group2 };
        var review = new Review(_newUser1, "Title", 5, "Description", ItemType.Book, groups);
        Assert.Equal(2, review.Groups.Count);
    }

    [Fact]
    public void TestAddGroupToReview()
    {
        var group1 = new Group("Group1", _newUser1);
        var review = new Review(_newUser1, "Title", 5, "Description", ItemType.Book);
        review.UpdateGroups([group1]);
        Assert.Contains(group1, review.Groups);
    }

    [Fact]
    public void TestRemoveGroupFromReview()
    {
        var group1 = new Group("Group1", _newUser1);
        var group2 = new Group("Group2", _newUser1);
        var groups = new List<Group> { group1 };
        var review = new Review(_newUser1, "Title", 5, "Description", ItemType.Book, groups);
        review.UpdateGroups([group1, group2]);
        review.UpdateGroups([group2]);
        Assert.DoesNotContain(group1, review.Groups);
    }

    [Fact]
    public void TestUpdateGroupsAddsMissingGroups()
    {
        var group1 = new Group("Group1", _newUser1);
        var group2 = new Group("Group2", _newUser1);
        var groups = new List<Group> { group1 };
        var review = new Review(_newUser1, "Title", 5, "Description", ItemType.Book, groups);
        review.UpdateGroups([group1, group2]);
        Assert.Contains(group2, review.Groups);
    }

    [Fact]
    public void TestUpdateGroupsRemovesExtraGroups()
    {
        var group1 = new Group("Group1", _newUser1);
        var group2 = new Group("Group2", _newUser1);
        var groups = new List<Group> { group1, group2 };
        var review = new Review(_newUser1, "Title", 5, "Description", ItemType.Book, groups);
        review.UpdateGroups([group1]);
        Assert.DoesNotContain(group2, review.Groups);
    }

    [Fact]
    public void TestChangeTitleUpdatesTitle()
    {
        var review = new Review(_newUser1, "OriginalTitle", 5, "Description", ItemType.Book);
        review.ChangeTitle("NewTitle");
        Assert.Equal("NewTitle", review.Title);
    }

    [Fact]
    public void TestChangeRatingUpdatesRating()
    {
        var review = new Review(_newUser1, "Title", 5, "Description", ItemType.Book);
        review.ChangeRating(3.5);
        Assert.Equal(3.5, review.Rating);
    }

    [Fact]
    public void TestChangeDescriptionUpdatesDescription()
    {
        var review = new Review(_newUser1, "Title", 5, "OriginalDesc", ItemType.Book);
        review.ChangeDescription("NewDesc");
        Assert.Equal("NewDesc", review.Description);
    }

    [Fact]
    public void TestChangeItemTypeUpdatesItemType()
    {
        var review = new Review(_newUser1, "Title", 5, "Description", ItemType.Book);
        review.ChangeItemType(ItemType.Film);
        Assert.Equal(ItemType.Film, review.ItemType);
    }

    [Fact]
    public void TestEnsureCanEditSucceedsForCreator()
    {
        var review = new Review(_newUser1, "Title", 5, "Description", ItemType.Book);
        var ex = Record.Exception(() => review.EnsureCanEdit(_newUser1));
        Assert.Null(ex);
    }

    [Fact]
    public void TestEnsureCanEditThrowsForOtherUser()
    {
        var otherUser = new User("Other", "other@sogyo.nl", "other", "user");
        var review = new Review(_newUser1, "Title", 5, "Description", ItemType.Book);
        Assert.Throws<UnauthorizedAccessException>(() => review.EnsureCanEdit(otherUser));
    }

    [Fact]
    public void TestUpdateGroupsWithNull()
    {
        var review = new Review(_newUser1, "Title", 5, "Description", ItemType.Book);
        review.UpdateGroups(null!);
        Assert.Empty(review.Groups);
    }

    [Fact]
    public void TestChangeLastUpdatedUpdatesTimestamp()
    {
        var review = new Review(_newUser1, "Title", 5, "Description", ItemType.Book);
        var originalTime = review.LastUpdated;
        review.ChangeTitle("NewTitle");
        Assert.True(review.LastUpdated > originalTime);
    }
}
