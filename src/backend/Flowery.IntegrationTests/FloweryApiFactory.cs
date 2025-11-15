using Flowery.Migrations;
using Flowery.WebApi.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Testcontainers.PostgreSql;

namespace Flowery.IntegrationTests;

public sealed class FloweryApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _pgContainer = new PostgreSqlBuilder()
        .WithUsername("flower")
        .WithPassword("password")
        .WithDatabase("flowery")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(x =>
        {
            x.Remove(x.Single(d => d.ServiceType == typeof(IDbConnectionFactory)));
            x.AddSingleton<IDbConnectionFactory>(_ =>
                new NpgsqlConnectionFactory(_pgContainer.GetConnectionString() ??
                                            throw new Exception("Connection string is not configured.")));
        });
    }

    public async ValueTask InitializeAsync()
    {
        await _pgContainer.StartAsync();
        var connectionString = _pgContainer.GetConnectionString();
        await RunMigrations(connectionString);
        await SeedData(connectionString);
    }

    public new async ValueTask DisposeAsync()
    {
        await _pgContainer.StopAsync();
    }

    public HttpClient GetClientByPath(string path)
    {
        return CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri(Server.BaseAddress, $"/api/v1/{path}/")
        });
    }

    private async ValueTask RunMigrations(string connectionString)
    {
        var migrationsPath = Path.Combine(
            AppContext.BaseDirectory,
            "..", "..", "..", "..",
            "Flowery.Migrations",
            "Migrations"
        );


        var migrationResult = MigrationsRunner.Run(connectionString, migrationsPath);
        if (!migrationResult.Successful)
        {
            await DisposeAsync();
            Environment.FailFast("Failed to migrate database.");
        }
    }

    private static async ValueTask SeedData(string connectionString)
    {
        var seedPath = Path.Combine(
            AppContext.BaseDirectory,
            "..", "..", "..",
            "Seeding"
        );

        var scripts = Directory.GetFiles(seedPath, "*.sql")
            .OrderBy(Path.GetFileName)
            .ToArray();

        await using var connection = new NpgsqlConnection(connectionString);
        await connection.OpenAsync();

        foreach (var scriptPath in scripts)
        {
            var sql = await File.ReadAllTextAsync(scriptPath);

            await using var cmd = connection.CreateCommand();
            cmd.CommandText = sql;

            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to execute SQL script: {Path.GetFileName(scriptPath)}", ex);
            }
        }
    }
}