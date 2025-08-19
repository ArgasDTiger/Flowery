using Flowery.WebApi.Infrastructure.Data;

namespace Flowery.WebApi.Infrastructure;

public static class InfrastructureDependencies
{
    public static void AddInfrastructureDependencies(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<IDbConnectionFactory>(_ =>
            new NpgsqlConnectionFactory(config.GetConnectionString("Postgres") ??
                                        throw new Exception("Connection string is not configured.")));
    }
}