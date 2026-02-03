using Flowery.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Flowery.Shared;

public static class Dependencies
{
    public static void AddDomainDependencies(this IServiceCollection services)
    {
        services.AddSingleton<ITimeService, TimeService>();
    }
}