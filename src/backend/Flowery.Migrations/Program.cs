using System.Reflection;
using DbUp;
using DbUp.Engine.Output;
using Microsoft.Extensions.Configuration;
using Serilog;
using Npgsql;

const string defaultSchema = "public";

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var config = new ConfigurationBuilder()
        .AddCommandLine(args)
        .Build();

    var environmentName = config["environment"] ?? "Development";

    var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

    if (string.IsNullOrEmpty(basePath))
    {
        Log.Error("Cannot find the base path.");
        throw new Exception("Cannot find the base path.");
    }
    
    var appsettings = new ConfigurationBuilder()
        .SetBasePath(basePath)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
        .Build();

    var connectionString = appsettings.GetSection("ConnectionStrings:Postgres").Value;

    if (string.IsNullOrEmpty(connectionString))
    {
        Log.Error("Connection string for the environment '{EnvironmentName}' is not configured.", environmentName);
        throw new Exception($"Connection string is not initialized for environment: {environmentName}.");
    }
    
    Log.Information("Migrating PostgreSQL database for environment '{EnvironmentName}'.", environmentName);

    EnsureDatabase.For.PostgresqlDatabase(connectionString);

    var migrationsPath = Path.Combine(basePath, "..", "..", "..", "Migrations");
    using (var connection = new NpgsqlConnection(connectionString))
    {
        connection.Open();
        using (var command = connection.CreateCommand())
        {
            var schema = config["schema"] ?? defaultSchema;
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

    var result = upgrader.PerformUpgrade();

    if (!result.Successful)
    {
        Log.Fatal(result.Error, "An error occurred while migrating the postgresql database.");
        return -1;
    }
    
    Log.Information("Postgresql database migration completed successfully.");
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "The application terminated unexpectedly.");
    return -1;
}
finally
{
    Log.CloseAndFlush();
}