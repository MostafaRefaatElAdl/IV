using Inovola.Application.Interfaces;
using Inovola.Application.Services;
using Inovola.Domain.Interfaces;
using Inovola.Infrastructure.Auth;
using Inovola.Infrastructure.Caching;
using Inovola.Infrastructure.Persistence;
using Inovola.Infrastructure.Persistence.Repositories;
using Inovola.Infrastructure.Weather;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Inovola.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();
        return services;
    }
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();

        // Services
        services.AddScoped<IWeatherService, MockWeatherService>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<WeatherAppService>();
        services.AddMemoryCache();
        services.AddScoped<ICacheService, MemoryCacheService>();
        return services;
    }
}
