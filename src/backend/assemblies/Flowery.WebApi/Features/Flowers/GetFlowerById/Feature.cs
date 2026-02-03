using Flowery.WebApi.Shared.Features;
using Microsoft.AspNetCore.Mvc;

namespace Flowery.WebApi.Features.Flowers.GetFlowerById;

public sealed class GetFlowerByIdFeature : IFeature
{
    public static void Register(IServiceCollection services)
    {
        services.AddSingleton<IQuery, Query>();
        services.AddScoped<IHandler, Handler>();
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/v1/flowers/id/{flowerId:guid}", async ([FromServices] IHandler handler,
                [FromServices] ILogger<GetFlowerByIdFeature> logger,
                [FromRoute] Guid flowerId,
                CancellationToken cancellationToken) =>
            {
                await HandleGetFlower(flowerId, logger, handler, cancellationToken);
            })
            .Produces<Response>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Gets a flower by Id.")
            .WithTags("Flowers");

        endpoints.MapGet("api/v1/flowers/id/{flowerId}", async ([FromServices] IHandler handler,
                    [FromServices] ILogger<GetFlowerByIdFeature> logger,
                    [FromRoute] string flowerId,
                    CancellationToken cancellationToken) =>
                await HandleGetFlower(flowerId, logger, handler, cancellationToken))
            .Produces<Response>()
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Gets a flower by slug.")
            .WithTags("Flowers");
    }

    private static async Task<IResult> HandleGetFlower<T>(T flowerId, ILogger<GetFlowerByIdFeature> logger,
        IHandler handler, CancellationToken cancellationToken)
    {
        try
        {
            var result = await handler.GetFlower(flowerId, cancellationToken);
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