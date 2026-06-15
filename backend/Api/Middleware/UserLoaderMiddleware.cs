using Application.Interfaces;

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
        ICurrentUserService currentUserService)
    {
        if (context.Request.Cookies.TryGetValue("UserId", out string? cookieValue))
        {
            if (Guid.TryParse(cookieValue, out Guid userId))
            {
                await currentUserService.SetUserByIdAsync(userId);
            }
        }
        await _next(context);
    }
}
