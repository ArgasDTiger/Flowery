using Flowery.WebApi.Features.Flowers.Helpers;

namespace Flowery.WebApi.Features.Flowers;

public static class Dependencies
{
    // TODO: create with code generators
    public static void AddFlowersDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IFlowerImageProcessor, FlowerImageProcessor>();
    }
}