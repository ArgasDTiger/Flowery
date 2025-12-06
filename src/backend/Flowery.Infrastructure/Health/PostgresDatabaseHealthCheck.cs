using Flowery.Infrastructure.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Flowery.Infrastructure.Health;

internal sealed class PostgresDatabaseHealthCheck : IHealthCheck
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public PostgresDatabaseHealthCheck(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
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
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}