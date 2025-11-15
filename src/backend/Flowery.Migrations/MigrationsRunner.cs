using DbUp;
using DbUp.Engine;
using DbUp.Engine.Output;
using Npgsql;

namespace Flowery.Migrations;

public static class MigrationsRunner
{
    private const string DefaultSchema = "public";

    public static DatabaseUpgradeResult Run(string connectionString, string migrationsPath, string schema = DefaultSchema)
    {
        if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString));
        
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = $"CREATE SCHEMA IF NOT EXISTS {schema};";
                command.ExecuteNonQuery();
            }
        }

        var upgrader = DeployChanges.To
            .PostgresqlDatabase(connectionString)
            .WithScriptsFromFileSystem(migrationsPath)
            .WithExecutionTimeout(TimeSpan.FromMinutes(5))
            .LogTo(new ConsoleUpgradeLog())
            .Build();

        return upgrader.PerformUpgrade();
    }
}