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
        endpoints.MapDelete("api/v1/flowers/id/{flowerId:guid}",
                async ([FromServices] IHandler handler,
                        [FromServices] ILogger<DeleteFlowerFeature> logger,
                        [FromRoute] Guid flowerId,
                        CancellationToken cancellationToken) =>
                    await HandleDeleteFlower(flowerId, logger, handler, cancellationToken))
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Deletes an existing flower.")
            .WithTags("Flowers");

        endpoints.MapDelete("api/v1/flowers/slug/{flowerId}",
                async ([FromServices] IHandler handler,
                        [FromServices] ILogger<DeleteFlowerFeature> logger,
                        [FromRoute] string flowerId,
                        CancellationToken cancellationToken) =>
                    await HandleDeleteFlower(flowerId, logger, handler, cancellationToken))
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Deletes an existing flower.")
            .WithTags("Flowers");
    }

    private static async Task<IResult> HandleDeleteFlower<T>(T flowerId, ILogger<DeleteFlowerFeature> logger,
        IHandler handler, CancellationToken cancellationToken)
    {
        try
        {
            var result = await handler.DeleteFlower(flowerId, cancellationToken);
            return result.Match(
                Results.Ok,
                _ => Results.NotFound());
        }
        catch (Exception ex)
        {
            logger.LogError("Error occured while getting flower: {Message}", ex.Message);
            return Results.InternalServerError();
        }
    }
}