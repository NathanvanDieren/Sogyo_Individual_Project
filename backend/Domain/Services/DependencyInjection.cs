using Microsoft.Extensions.DependencyInjection;
using Domain.Services;
using Domain.Interfaces;

namespace Domain.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<TokenService>();
        
        services.AddScoped<IUserFacade, UserFacade>();
        
        services.AddScoped<IReviewFacade, ReviewFacade>();
        
        services.AddScoped<IGroupFacade, GroupFacade>();

        return services;
    }
}