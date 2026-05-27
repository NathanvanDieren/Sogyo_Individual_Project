using Domain;
using Xunit;

namespace Api.Tests;

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
        User secondUser = new User("Nathan", "nvdieren@sogyo.nl", "nvdieren", "user");  
        Assert.NotEqual(newUser.Id, secondUser.Id);
    }
}