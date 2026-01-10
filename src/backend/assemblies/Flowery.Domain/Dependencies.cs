using Flowery.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Flowery.Domain;

public static class Dependencies
{
    public static void AddDomainDependencies(this IServiceCollection services)
    {
        services.AddSingleton<ITimeService, TimeService>();
    }
}