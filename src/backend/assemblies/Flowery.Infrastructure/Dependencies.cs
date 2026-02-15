using Flowery.Infrastructure.Auth;
using Flowery.Infrastructure.Auth.Passwords;
using Flowery.Infrastructure.Auth.Tokens;
using Flowery.Infrastructure.Data;
using Flowery.Infrastructure.Health;
using Flowery.Infrastructure.Images;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Flowery.Infrastructure;

public static class Dependencies
{
    extension(IServiceCollection services)
    {
        public void AddInfrastructureDependencies(IConfiguration config)
        {
            services.AddSingleton<IDbConnectionFactory>(_ =>
                new NpgsqlConnectionFactory(config.GetConnectionString("Postgres") ??
                                            throw new Exception("Connection string is not configured.")));
            services.AddHealthChecks()
                .AddCheck<PostgresDatabaseHealthCheck>("Database");

            services.AddAuthentication();
            services.AddConfigurations(config);
            services.AddHttpContextAccessor();

            services.AddSingleton<IImageProcessor, ImageProcessor>();
        }

        private void AddAuthentication()
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

        private void AddConfigurations(IConfiguration config)
        {
            services.AddOptions<AuthConfiguration>()
                .Bind(config.GetSection(nameof(AuthConfiguration)))
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }
    }
}