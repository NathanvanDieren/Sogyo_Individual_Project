using Domain;
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
}