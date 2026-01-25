using Flowery.Infrastructure.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Flowery.Infrastructure.Health;

internal sealed class PostgresDatabaseHealthCheck : IHealthCheck
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ILogger<PostgresDatabaseHealthCheck> _logger;

    public PostgresDatabaseHealthCheck(IDbConnectionFactory dbConnectionFactory, ILogger<PostgresDatabaseHealthCheck> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT 1";
            await command.ExecuteScalarAsync(cancellationToken);
            _logger.LogInformation("Returning healthy response from Postgres database.");
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Returning unhealthy response from Postgres database.");
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}