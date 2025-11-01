using Flowery.WebApi.Shared.Pagination;
using Flowery.WebApi.Shared.Settings;
using FluentValidation;

namespace Flowery.WebApi.Shared;

public static class SharedFeaturesDependencies
{
    public static IServiceCollection AddSharedFeatures(this IServiceCollection services)
    {
        services.AddScoped<IValidator<PaginationParams>, PaginationParamsValidator>();

        return services;
    }

    public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration config)
    {
        services.AddOptions<TranslationSettings>()
            .Bind(config.GetSection(nameof(TranslationSettings)))
            .ValidateDataAnnotations();
        
        return services;
    }
}