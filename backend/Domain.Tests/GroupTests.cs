using Domain.Classes;
using Xunit;

namespace Domain.Tests;

public class GroupTests
{
    private User _newUser1 = new User("Nathan", "nvdieren@sogyo.nl", "nvdieren", "user");
    private User _newUser2 = new User("Bart", "bart@sogyo.nl", "bart", "user");

    [Fact]
    public void TestNewGroup()
    {
        var newGroup = new Group("Group1", _newUser1);
        Assert.NotNull(newGroup);
    }

    [Fact]
    public void TestAddingUserToGroup()
    {
        var newGroup = new Group("Group1", _newUser1);
        newGroup.AddMember(_newUser2);
        Assert.Equal(2, newGroup.Members.Count);
    }

    [Fact]
    public void TestRemovingUserFromGroup()
    {
        var newGroup = new Group("Group1", _newUser1);
        newGroup.AddMember(_newUser2);
        newGroup.RemoveMember(_newUser2);
        Assert.Single(newGroup.Members);
    }

    [Fact]
    public void TestRemovingUserThatDoesNotExist()
    {
        var newGroup = new Group("Group1", _newUser1);
        newGroup.RemoveMember(_newUser2);
        Assert.Single(newGroup.Members);
    }

    [Fact]
    public void TestGroupNameIsSetCorrectly()
    {
        var newGroup = new Group("TestGroup", _newUser1);
        Assert.Equal("TestGroup", newGroup.Name);
    }

    [Fact]
    public void TestCreatorIdIsSetCorrectly()
    {
        var newGroup = new Group("Group1", _newUser1);
        Assert.Equal(_newUser1.Id, newGroup.CreatorId);
    }

    [Fact]
    public void TestCreatorIsSetCorrectly()
    {
        var newGroup = new Group("Group1", _newUser1);
        Assert.Equal(_newUser1, newGroup.Creator);
    }

    [Fact]
    public void TestCreatorIsAddedAsMember()
    {
        var newGroup = new Group("Group1", _newUser1);
        Assert.Contains(_newUser1, newGroup.Members);
    }

    [Fact]
    public void TestAddMemberDoesNotAddNull()
    {
        var newGroup = new Group("Group1", _newUser1);
        newGroup.AddMember(null!);
        Assert.Single(newGroup.Members);
    }

    [Fact]
    public void TestAddMemberDoesNotAddDuplicate()
    {
        var newGroup = new Group("Group1", _newUser1);
        newGroup.AddMember(_newUser1);
        Assert.Single(newGroup.Members);
    }

    [Fact]
    public void TestRemoveMemberDoesNotThrowOnNull()
    {
        var newGroup = new Group("Group1", _newUser1);
        newGroup.RemoveMember(null!);
        Assert.Single(newGroup.Members);
    }

    [Fact]
    public void TestChangeNameUpdatesName()
    {
        var newGroup = new Group("Group1", _newUser1);
        newGroup.ChangeName("NewName");
        Assert.Equal("NewName", newGroup.Name);
    }

    [Fact]
    public void TestEnsureCanEditSucceedsForCreator()
    {
        var newGroup = new Group("Group1", _newUser1);
        var ex = Record.Exception(() => newGroup.EnsureCanEdit(_newUser1));
        Assert.Null(ex);
    }

    [Fact]
    public void TestEnsureCanEditThrowsForOtherUser()
    {
        var newGroup = new Group("Group1", _newUser1);
        Assert.Throws<UnauthorizedAccessException>(() => newGroup.EnsureCanEdit(_newUser2));
    }

    [Fact]
    public void TestUpdateMembersWithNewAndRemovedMembers()
    {
        var newGroup = new Group("Group1", _newUser1);
        newGroup.AddMember(_newUser2);
        User _newUser3 = new User("Third", "third@sogyo.nl", "third", "user");
        newGroup.UpdateMembers([_newUser1, _newUser3]);
        Assert.DoesNotContain(_newUser2, newGroup.Members);
    }

    [Fact]
    public void TestUpdateMembersAddsMissingMembers()
    {
        var newGroup = new Group("Group1", _newUser1);
        newGroup.UpdateMembers([_newUser1, _newUser2]);
        Assert.Contains(_newUser2, newGroup.Members);
    }

    [Fact]
    public void TestUpdateMembersPreservesCreator()
    {
        var newGroup = new Group("Group1", _newUser1);
        newGroup.UpdateMembers([_newUser2]);
        Assert.Contains(_newUser1, newGroup.Members);
    }

    [Fact]
    public void TestUpdateMembersWithNull()
    {
        var newGroup = new Group("Group1", _newUser1);
        newGroup.UpdateMembers(null!);
        Assert.Single(newGroup.Members);
    }

    [Fact]
    public void TestChangeLastUpdatedUpdatesTimestamp()
    {
        var newGroup = new Group("Group1", _newUser1);
        DateTime originalTime = newGroup.LastUpdated;
        newGroup.ChangeLastUpdated();
        Assert.True(newGroup.LastUpdated > originalTime);
    }
}
