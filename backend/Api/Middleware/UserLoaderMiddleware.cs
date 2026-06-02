using Domain;
using Domain.Services; 
using Domain.Interfaces;

namespace Api.Middleware;

public class UserLoaderMiddleware
{
    private readonly RequestDelegate _next;

    public UserLoaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(
        HttpContext context, 
        IUserRepository userRepository, 
        CurrentUserService currentUserService)
    {
        if (context.Request.Cookies.TryGetValue("UserId", out string? cookieValue))
        {
            if (Guid.TryParse(cookieValue, out Guid userId))
            {
                var user = await userRepository.GetUserById(userId);
                
                if (user != null)
                {
                    currentUserService.SetCurrentUser(user); 
                }
            }
        }
        await _next(context);
    }
}