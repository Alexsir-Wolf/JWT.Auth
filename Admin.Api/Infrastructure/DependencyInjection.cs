using Admin.Api.Repositories;
using Admin.Api.Repositories.Interfaces;
using Admin.Api.Services;
using Admin.Api.Services.Interfaces;

namespace Admin.Api.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}