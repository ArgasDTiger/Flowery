using Flowery.WebApi.Infrastructure.Auth;
using Flowery.WebApi.Infrastructure.Auth.Passwords;
using Flowery.WebApi.Infrastructure.Auth.Tokens;
using Flowery.WebApi.Infrastructure.Configurations;
using Flowery.WebApi.Infrastructure.Data;
using Flowery.WebApi.Infrastructure.Health;
using Microsoft.AspNetCore.Identity;

namespace Flowery.WebApi.Infrastructure;

public static class InfrastructureDependencies
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