using Flowery.WebApi.Shared.Features;
using Microsoft.AspNetCore.Mvc;

namespace Flowery.WebApi.Features.Flowers.GetFlowerById;

public sealed class GetFlowerByIdFeature : IFeature
{
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<IQuery, Query>();
        services.AddScoped<IHandler, Handler>();
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/v1/flowers/{flowerId}", async ([FromServices] IHandler handler,
            [FromServices] ILogger<GetFlowerByIdFeature> logger,
            [FromRoute] string flowerId,
            CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await handler.GetFlowerById(flowerId, cancellationToken);
                return result.Match(
                    Results.Ok,
                    _ => Results.NotFound());
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting flower: {Message}", ex.Message);
                return Results.InternalServerError();
            }
        });
    }
}