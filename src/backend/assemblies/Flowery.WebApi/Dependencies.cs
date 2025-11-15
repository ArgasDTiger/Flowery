using Flowery.WebApi.Infrastructure;

namespace Flowery.WebApi;

public static class Dependencies
{
    public static void AddServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddInfrastructureDependencies(config);
    }
}