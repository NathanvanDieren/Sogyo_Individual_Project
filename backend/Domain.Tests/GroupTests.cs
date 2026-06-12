using Domain;
using Domain.Exceptions;
using Xunit;

namespace Domain.Tests;

public class GroupTests
{
    private User _newUser1 = new User("Nathan", "nvdieren@sogyo.nl", "nvdieren", "user");
    private User _newUser2 = new User("Bart", "bart@sogyo.nl", "bart", "user");
    
    [Fact]
    public void TestNewGroup()
    {
        Group newGroup = new Group("Group1", _newUser1);
        Assert.NotNull(newGroup);
    }

    [Fact]
    public void TestAddingUserToGroup()
    {
        Group newGroup = new Group("Group1", _newUser1);
        newGroup.AddMember(_newUser2);
        Assert.Equal(2, newGroup.Members.Count);
    }
    
    [Fact]
    public void TestRemovingUserFromGroup()
    {
        Group newGroup = new Group("Group1", _newUser1);
        newGroup.AddMember(_newUser2);
        newGroup.RemoveMember(_newUser2);
        Assert.Single(newGroup.Members);
    }
    
    [Fact]
    public void TestRemovingUserThatDoesNotExist()
    {
        Group newGroup = new Group("Group1", _newUser1);
        newGroup.RemoveMember(_newUser2);
        Assert.Single(newGroup.Members);
    }

    [Fact]
    public void TestGroupNameIsSetCorrectly()
    {
        Group newGroup = new Group("TestGroup", _newUser1);
        Assert.Equal("TestGroup", newGroup.Name);
    }

    [Fact]
    public void TestCreatorIdIsSetCorrectly()
    {
        Group newGroup = new Group("Group1", _newUser1);
        Assert.Equal(_newUser1.Id, newGroup.CreatorId);
    }

    [Fact]
    public void TestCreatorIsSetCorrectly()
    {
        Group newGroup = new Group("Group1", _newUser1);
        Assert.Equal(_newUser1, newGroup.Creator);
    }

    [Fact]
    public void TestCreatorIsAddedAsMember()
    {
        Group newGroup = new Group("Group1", _newUser1);
        Assert.Contains(_newUser1, newGroup.Members);
    }

    [Fact]
    public void TestAddMemberDoesNotAddNull()
    {
        Group newGroup = new Group("Group1", _newUser1);
        newGroup.AddMember(null!);
        Assert.Single(newGroup.Members);
    }

    [Fact]
    public void TestAddMemberDoesNotAddDuplicate()
    {
        Group newGroup = new Group("Group1", _newUser1);
        newGroup.AddMember(_newUser1);
        Assert.Single(newGroup.Members);
    }

    [Fact]
    public void TestRemoveMemberDoesNotThrowOnNull()
    {
        Group newGroup = new Group("Group1", _newUser1);
        newGroup.RemoveMember(null!);
        Assert.Single(newGroup.Members);
    }

    [Fact]
    public void TestChangeNameUpdatesName()
    {
        Group newGroup = new Group("Group1", _newUser1);
        newGroup.ChangeName("NewName");
        Assert.Equal("NewName", newGroup.Name);
    }

    [Fact]
    public void TestEnsureCanEditSucceedsForCreator()
    {
        Group newGroup = new Group("Group1", _newUser1);
        var ex = Record.Exception(() => newGroup.EnsureCanEdit(_newUser1));
        Assert.Null(ex);
    }

    [Fact]
    public void TestEnsureCanEditThrowsForOtherUser()
    {
        Group newGroup = new Group("Group1", _newUser1);
        Assert.Throws<UnauthorizedDomainException>(() => newGroup.EnsureCanEdit(_newUser2));
    }

    [Fact]
    public void TestUpdateMembersWithNewAndRemovedMembers()
    {
        Group newGroup = new Group("Group1", _newUser1);
        newGroup.AddMember(_newUser2);
        User _newUser3 = new User("Third", "third@sogyo.nl", "third", "user");
        newGroup.UpdateMembers([_newUser1, _newUser3]);
        Assert.DoesNotContain(_newUser2, newGroup.Members);
    }

    [Fact]
    public void TestUpdateMembersAddsMissingMembers()
    {
        Group newGroup = new Group("Group1", _newUser1);
        newGroup.UpdateMembers([_newUser1, _newUser2]);
        Assert.Contains(_newUser2, newGroup.Members);
    }

    [Fact]
    public void TestUpdateMembersPreservesCreator()
    {
        Group newGroup = new Group("Group1", _newUser1);
        newGroup.UpdateMembers([_newUser2]);
        Assert.Contains(_newUser1, newGroup.Members);
    }

    [Fact]
    public void TestUpdateMembersWithNull()
    {
        Group newGroup = new Group("Group1", _newUser1);
        newGroup.UpdateMembers(null!);
        Assert.Single(newGroup.Members);
    }

    [Fact]
    public void TestChangeLastUpdatedUpdatesTimestamp()
    {
        Group newGroup = new Group("Group1", _newUser1);
        DateTime originalTime = newGroup.LastUpdated;
        newGroup.ChangeLastUpdated();
        Assert.True(newGroup.LastUpdated > originalTime);
    }
}
