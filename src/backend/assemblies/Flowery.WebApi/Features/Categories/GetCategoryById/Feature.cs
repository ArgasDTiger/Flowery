using Flowery.WebApi.Shared.Features;
using Microsoft.AspNetCore.Mvc;

namespace Flowery.WebApi.Features.Categories.GetCategoryById;

public sealed class GetCategoryByIdFeature : IFeature
{
    public static void Register(IServiceCollection services)
    {
        services.AddSingleton<IQuery, Query>();
        services.AddScoped<IHandler, Handler>();
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/v1/category/{slug}", async (
                [FromServices] IHandler handler,
                [FromServices] ILogger<GetCategoryByIdFeature> logger,
                [FromRoute] string slug,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    var result = await handler.GetCategoryBySlug(slug, cancellationToken);
                    return result.Match(
                        category => Results.Ok(category),
                        _ => Results.NotFound());
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error occured while getting category: {Message}", ex.Message);
                    return Results.InternalServerError();
                }
            })
            .Produces<Response>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Gets a category by slug.")
            .WithTags("Categories");
    }
}