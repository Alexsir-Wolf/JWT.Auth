using Admin.Api.Infrastructure.Interfaces;
using Admin.Api.Infrastructure.Services;
using Admin.Api.Repositories.Interfaces;
using Admin.Api.Services.Interfaces;
using Admin.Api.Repositories;
using Admin.Api.Services;

namespace Admin.Api.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordService, PasswordService>();

        return services;
    }
}