using Flowery.Infrastructure.Data;
using Flowery.Migrations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace Flowery.IntegrationTests.TestHelpers.ApiFactories;

public abstract class BaseApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    protected readonly PostgreSqlContainer PgContainer = new PostgreSqlBuilder()
        .WithUsername("flower")
        .WithPassword("password")
        .WithDatabase("flowery_writable")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(x =>
        {
            x.Remove(x.Single(d => d.ServiceType == typeof(IDbConnectionFactory)));
            x.AddSingleton<IDbConnectionFactory>(_ =>
                new NpgsqlConnectionFactory(ConnectionString ??
                                            throw new Exception("Connection string is not configured.")));
        });
    }

    public virtual async ValueTask InitializeAsync()
    {
        await PgContainer.StartAsync();
        await RunMigrations();
    }

    public HttpClient GetClientByPath(string path)
    {
        return CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri(Server.BaseAddress, $"/api/v1/{path}/")
        });
    }

    protected string ConnectionString => PgContainer.GetConnectionString();

    private async ValueTask RunMigrations()
    {
        var migrationsPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "tools",
            "Flowery.Migrations", "Migrations");

        var migrationResult = MigrationsRunner.Run(ConnectionString, migrationsPath);
        if (!migrationResult.Successful)
        {
            await DisposeAsync();
            Environment.FailFast("Failed to migrate database.");
        }
    }
}