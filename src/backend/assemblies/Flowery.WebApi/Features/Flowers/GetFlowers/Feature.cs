using Flowery.Shared.Enums;
using Flowery.WebApi.Shared.Features;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Flowery.WebApi.Features.Flowers.GetFlowers;

public sealed class GetFlowersFeature : IFeature
{
    public static void Register(IServiceCollection services)
    {
        services.AddSingleton<Query>();
        services.AddSingleton<Handler>();
        services.AddSingleton<IValidator<Request>, RequestValidator>();
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/v1/flowers",
                async ([FromServices] Handler handler,
                    [FromServices] IValidator<Request> validator,
                    [FromServices] ILogger<GetFlowersFeature> logger,
                    [AsParameters] Request request,
                    CancellationToken cancellationToken) =>
                {
                    try
                    {
                        // TODO: detect lang
                        var responses = await handler.GetFlowers(request, LanguageCode.UA, cancellationToken);
                        return Results.Ok(responses);
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Error occured while getting flowers: {Message}", e.Message);
                        return Results.InternalServerError();
                    }
                })
            .Produces<Response[]>()
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status500InternalServerError)
            .WithValidation<Request>()
            .WithSummary("Gets all flowers.")
            .WithTags("Flowers");
    }
}