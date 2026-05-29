using Microsoft.Extensions.DependencyInjection;
using Domain.Services;

namespace Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<TokenService>();
        
        services.AddScoped<IUserFacade, UserFacade>();

        return services;
    }
}