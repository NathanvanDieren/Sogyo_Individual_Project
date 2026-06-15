using Domain.Classes;
using Xunit;

namespace Domain.Tests;

public class UserTest
{
    private User newUser = new User("Nathan", "nvdieren@sogyo.nl", "nvdieren", "user");

    [Fact]
    public void TestNewUserNotNull()
    {
        Assert.NotNull(newUser);
    }

    [Fact]
    public void TestDifferentUsersGetDifferentId()
    {
        var secondUser = new User("Nathan", "nvdieren@sogyo.nl", "nvdieren", "user");
        Assert.NotEqual(newUser.Id, secondUser.Id);
    }

    [Fact]
    public void TestUsernameIsSetCorrectly()
    {
        Assert.Equal("Nathan", newUser.Username);
    }

    [Fact]
    public void TestEmailIsNormalizedToLowercase()
    {
        var user = new User("TestUser", "UPPER@CASE.COM", "password", "admin");
        Assert.Equal("upper@case.com", user.Email);
    }

    [Fact]
    public void TestPasswordHashIsSetCorrectly()
    {
        Assert.Equal("nvdieren", newUser.PasswordHash);
    }

    [Fact]
    public void TestRoleIdIsSetCorrectly()
    {
        Assert.Equal("user", newUser.RoleId);
    }

    [Fact]
    public void TestChangeUsernameReturnsFalseWhenSame()
    {
        Assert.False(newUser.ChangeUsername("Nathan"));
    }

    [Fact]
    public void TestChangeUsernameReturnsTrueWhenDifferent()
    {
        Assert.True(newUser.ChangeUsername("NewName"));
    }

    [Fact]
    public void TestChangeUsernameTrimsWhitespace()
    {
        newUser.ChangeUsername("  NewName  ");
        Assert.Equal("NewName", newUser.Username);
    }
}
