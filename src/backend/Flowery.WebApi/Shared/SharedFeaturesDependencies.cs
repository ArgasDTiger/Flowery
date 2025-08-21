using Flowery.WebApi.Shared.Pagination;
using FluentValidation;

namespace Flowery.WebApi.Shared;

public static class SharedFeaturesDependencies
{
    public static IServiceCollection AddSharedFeatures(this IServiceCollection services)
    {
        services.AddScoped<IValidator<PaginationParams>, PaginationParamsValidator>();

        return services;
    }
}