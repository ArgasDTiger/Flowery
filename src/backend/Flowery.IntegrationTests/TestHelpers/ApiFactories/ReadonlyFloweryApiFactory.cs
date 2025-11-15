using Npgsql;

namespace Flowery.IntegrationTests.TestHelpers.ApiFactories;

public sealed class ReadonlyFloweryApiFactory : BaseApiFactory, IAsyncLifetime
{
    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync();
        await SeedData();
    }

    public new async ValueTask DisposeAsync()
    {
        await PgContainer.StopAsync();
    }

    private async ValueTask SeedData()
    {
        var seedPath = Path.Combine(
            AppContext.BaseDirectory,
            "..", "..", "..",
            "Seeding"
        );

        var scripts = Directory.GetFiles(seedPath, "*.sql")
            .OrderBy(Path.GetFileName)
            .ToArray();

        await using var connection = new NpgsqlConnection(ConnectionString);
        await connection.OpenAsync();

        foreach (var scriptPath in scripts)
        {
            var sql = await File.ReadAllTextAsync(scriptPath);
            await using var cmd = connection.CreateCommand();
            cmd.CommandText = sql;
            await cmd.ExecuteNonQueryAsync();
        }
    }
}