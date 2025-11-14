using Flowery.WebApi.Infrastructure.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Flowery.WebApi.Infrastructure.Health;

public sealed class PostgresDatabaseHealthCheck : IHealthCheck
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public PostgresDatabaseHealthCheck(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
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