using System.Reflection;
using DbUp;
using Flowery.Migrations;
using Microsoft.Extensions.Configuration;
using Serilog;

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

    var migrationResult = MigrationsRunner.Run(connectionString, migrationsPath);

    if (!migrationResult.Successful)
    {
        Log.Fatal(migrationResult.Error, "An error occurred while migrating the postgresql database.");
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