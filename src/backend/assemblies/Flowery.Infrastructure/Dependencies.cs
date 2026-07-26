using Azure.Identity;
using Flowery.Infrastructure.Auth;
using Flowery.Infrastructure.Auth.Passwords;
using Flowery.Infrastructure.Auth.Tokens;
using Flowery.Infrastructure.Data;
using Flowery.Infrastructure.Health;
using Flowery.Infrastructure.Images;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Flowery.Infrastructure;

public static class Dependencies
{
    extension(IServiceCollection services)
    {
        public void AddInfrastructureDependencies(IConfiguration config)
        {
            string connectionString = config.GetConnectionString("Postgres") ??
                                      throw new Exception("Connection string is not configured.");
            services.AddSingleton<IDbConnectionFactory>(_ =>
                new NpgsqlConnectionFactory(connectionString));
            services.AddHealthChecks()
                .AddCheck<PostgresDatabaseHealthCheck>("Database");

            services.AddAuthentication();
            services.AddConfigurations(config);
            services.AddHttpContextAccessor();

            services.AddSingleton<IImageProcessor, ImageProcessor>();
            services.AddSingleton<IImageRetrieval, AzureImageRetrieval>();
            services.AddSingleton<IImageSaver, AzureImageSaver>();

            services.AddHangfire(connectionString);

            services.ConfigureAzure();
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

        private void AddHangfire(string connectionString)
        {
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute
            {
                Attempts = 3
            });

            // TODO: probably better to use separate db with another, less privileged, user
            services.AddHangfire(config => config
                .UseSerilogLogProvider()
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(opt => opt.UseNpgsqlConnection(connectionString), 
                    new PostgreSqlStorageOptions
                    {
                        QueuePollInterval = TimeSpan.FromSeconds(20),
                        InvisibilityTimeout = TimeSpan.FromMinutes(5),
                        DistributedLockTimeout = TimeSpan.FromMinutes(1),
                    }));

            services.AddHangfireServer(options =>
            {
                options.ServerName = $"Flowery-{Environment.MachineName}";
            });
        }

        private void ConfigureAzure()
        {
            services.AddAzureClients(clientBuilder =>
            {
                clientBuilder.UseCredential(new DefaultAzureCredential());
                clientBuilder.AddBlobServiceClient(new Uri("https://flowery.blob.core.windows.net"));
            });
        }
    }
}