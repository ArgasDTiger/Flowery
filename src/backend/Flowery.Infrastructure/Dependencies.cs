using Flowery.Infrastructure.Auth;
using Flowery.Infrastructure.Auth.Passwords;
using Flowery.Infrastructure.Auth.Tokens;
using Flowery.Infrastructure.Data;
using Flowery.Infrastructure.Health;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Flowery.Infrastructure;

public static class Dependencies
{
    public static void AddInfrastructureDependencies(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IDbConnectionFactory>(_ =>
            new NpgsqlConnectionFactory(config.GetConnectionString("Postgres") ??
                                        throw new Exception("Connection string is not configured.")));
        services.AddHealthChecks()
            .AddCheck<PostgresDatabaseHealthCheck>("Database");

        services.AddAuthentication();
        services.AddConfigurations(config);
        services.AddHttpContextAccessor();
    }

    private static void AddAuthentication(this IServiceCollection services)
    {
        services.AddSingleton<IUserPasswordHasher, UserPasswordHasher>();
        services.AddSingleton<ITokenService, TokenService>();
        services.AddSingleton<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IAuthCookieService, AuthCookieService>();

        services.Configure<PasswordHasherOptions>(options =>
        {
            options.IterationCount = 600_000;
            options.CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3;
        });
    }

    private static void AddConfigurations(this IServiceCollection services, IConfiguration config)
    {
        services.AddOptions<AuthConfiguration>()
            .Bind(config.GetSection(nameof(AuthConfiguration)))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}