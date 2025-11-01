using Flowery.WebApi.Shared.Features;
using Microsoft.AspNetCore.Mvc;

namespace Flowery.WebApi.Features.Flowers.DeleteFlower;

public sealed class DeleteFlowerFeature : IFeature
{
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<IQuery, Query>();
        services.AddScoped<IHandler, Handler>();
    }

    // TODO: support deleting by slug
    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete("api/v1/flowers/{flowerId:guid}",
            async ([FromServices] IHandler handler,
                [FromServices] ILogger<DeleteFlowerFeature> logger,
                [FromRoute] Guid flowerId, 
                CancellationToken cancellationToken) =>
            {
                try
                {
                    var result = await handler.DeleteFlower(flowerId, cancellationToken);
                    return result.Match(
                        _ => Results.NoContent(),
                        _ => Results.NotFound());
                }
                catch (Exception ex)
                {
                    logger.LogError("Error occured while deleting flower: {Message}", ex.Message);
                    return Results.InternalServerError();
                }
                
            });
    }
}