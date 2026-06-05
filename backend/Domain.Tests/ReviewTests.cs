using Domain;
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

        string title = "Het Gouden Ei";
        string description = "Ik vond hem erg spannend en vlot geschreven.";
        int rating = 5;
        ItemType type = ItemType.Book;
        
        var review = new Review(_newUser1, title, rating, description, type, groups);
        
        Assert.NotNull(review);
        Assert.NotEqual(Guid.Empty, review.Id);
        Assert.Equal(title, review.Title);
        Assert.Equal(description, review.Description);
        Assert.Equal(type, review.ItemType);
        
        Assert.Single(review.Groups); 
        Assert.Contains(group1, review.Groups);
    }

    [Fact]
    public void TestAddingGroupToReview()
    {
        var group1 = new Group("Boekenclub", _newUser1);
        var group2 = new Group("Boekenclub2", _newUser1);
        var groups = new List<Group> { group1 };

        string title = "Het Gouden Ei";
        string description = "Ik vond hem erg spannend en vlot geschreven.";
        int rating = 5;
        ItemType type = ItemType.Book;
        
        var review = new Review(_newUser1, title, rating, description, type, groups);
        review.AddGroup(group2);
        Assert.Contains(group1, review.Groups);
    }
    
    [Fact]
    public void TestRemovingGroupFromReview()
    {
        var group1 = new Group("Boekenclub", _newUser1);
        var group2 = new Group("Boekenclub2", _newUser1);
        var groups = new List<Group> { group1 };

        string title = "Het Gouden Ei";
        string description = "Ik vond hem erg spannend en vlot geschreven.";
        int rating = 5;
        ItemType type = ItemType.Book;
        
        var review = new Review(_newUser1, title, rating, description, type, groups);
        review.AddGroup(group2);
        review.RemoveGroup(group2);
        Assert.DoesNotContain(group2, review.Groups);
    }

    
    [Fact]
    public void TestRemovingGroupThatDoesNotExist()
    {
        var group1 = new Group("Boekenclub", _newUser1);
        var group2 = new Group("Boekenclub2", _newUser1);
        var groups = new List<Group> { group1 };

        string title = "Het Gouden Ei";
        string description = "Ik vond hem erg spannend en vlot geschreven.";
        int rating = 5;
        ItemType type = ItemType.Book;
        
        var review = new Review(_newUser1, title, rating, description, type, groups);
        review.RemoveGroup(group2);
        Assert.DoesNotContain(group2, review.Groups);
    }
}