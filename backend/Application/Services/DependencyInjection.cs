using Application.Interfaces;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<TokenService>();

        services.AddScoped<IUserFacade, UserFacade>();

        services.AddScoped<IReviewFacade, ReviewFacade>();

        services.AddScoped<IGroupFacade, GroupFacade>();

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
