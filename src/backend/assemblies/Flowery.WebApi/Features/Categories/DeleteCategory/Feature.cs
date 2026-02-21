using Flowery.WebApi.Shared.Features;
using Microsoft.AspNetCore.Mvc;

namespace Flowery.WebApi.Features.Categories.DeleteCategory;

public sealed class DeleteCategoryFeature : IFeature
{
    public static void Register(IServiceCollection services)
    {
        services.AddSingleton<IQuery, Query>();
        services.AddScoped<IHandler, Handler>();
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete("api/v1/category/{slug}", async (
                [FromServices] IHandler handler,
                [FromServices] ILogger<DeleteCategoryFeature> logger,
                [FromRoute] string slug,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    var result = await handler.DeleteCategory(slug, cancellationToken);
                    return result.Match(
                        _ => Results.NoContent(),
                        _ => Results.NotFound());
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error occured while deleting category: {Message}", ex.Message);
                    return Results.InternalServerError();
                }
            })
            .DisableAntiforgery()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Deletes a category by slug.")
            .WithTags("Categories");
    }
}