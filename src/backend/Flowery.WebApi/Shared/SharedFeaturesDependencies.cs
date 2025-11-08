using Flowery.WebApi.Shared.Configurations;
using Flowery.WebApi.Shared.Pagination;
using FluentValidation;

namespace Flowery.WebApi.Shared;

public static class SharedFeaturesDependencies
{
    public static IServiceCollection AddSharedFeatures(this IServiceCollection services)
    {
        services.AddSingleton<IValidator<PaginationParams>, PaginationParamsValidator>();

        return services;
    }

    public static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration config)
    {
        services.AddOptions<TranslationConfiguration>()
            .Bind(config.GetSection(nameof(TranslationConfiguration)))
            .ValidateDataAnnotations();
        
        return services;
    }
}