using Flowery.WebApi.Shared.Configurations;
using Flowery.WebApi.Shared.Pagination;
using Flowery.WebApi.Shared.Services;
using FluentValidation;

namespace Flowery.WebApi.Shared;

public static class SharedFeaturesDependencies
{
    public static void AddSharedFeatures(this IServiceCollection services)
    {
        services.AddSingleton<IValidator<PaginationParams>, PaginationParamsValidator>();
        services.AddSingleton<IValidator<OrderedPaginationParams>, OrderedPaginationParamsValidator>();

        services.AddSingleton<ITimeService, TimeService>();
    }

    public static void AddConfigurations(this IServiceCollection services, IConfiguration config)
    {
        services.AddOptions<TranslationConfiguration>()
            .Bind(config.GetSection(nameof(TranslationConfiguration)))
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}