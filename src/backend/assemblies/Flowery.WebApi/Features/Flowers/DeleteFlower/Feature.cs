using Flowery.WebApi.Shared.Features;
using Microsoft.AspNetCore.Mvc;

namespace Flowery.WebApi.Features.Flowers.DeleteFlower;

public sealed class DeleteFlowerFeature : IFeature
{
    public static void Register(IServiceCollection services)
    {
        services.AddSingleton<IQuery, Query>();
        services.AddScoped<IHandler, Handler>();
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete("api/v1/flowers/{flowerId}",
                async ([FromServices] IHandler handler,
                    [FromServices] ILogger<DeleteFlowerFeature> logger,
                    [FromRoute] string slug, 
                    CancellationToken cancellationToken) =>
                {
                    try
                    {
                        var result = await handler.DeleteFlower(slug, cancellationToken);
                        return result.Match(
                            _ => Results.NoContent(),
                            _ => Results.NotFound());
                    }
                    catch (Exception ex)
                    {
                        logger.LogError("Error occured while deleting flower: {Message}", ex.Message);
                        return Results.InternalServerError();
                    }
                })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Deletes an existing flower.")
            .WithTags("Flowers");
    }
}