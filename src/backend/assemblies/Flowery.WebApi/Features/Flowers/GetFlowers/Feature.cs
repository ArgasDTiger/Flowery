using Flowery.WebApi.Shared.Features;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Flowery.WebApi.Features.Flowers.GetFlowers;

public sealed class GetFlowersFeature : IFeature
{
    public static void Register(IServiceCollection services)
    {
        services.AddSingleton<IQuery, Query>();
        services.AddScoped<IHandler, Handler>();
        services.AddSingleton<IValidator<Request>, RequestValidator>();
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/v1/flowers",
            async ([FromServices] IHandler handler,
                [FromServices] IValidator<Request> validator,
                [FromServices] ILogger<GetFlowersFeature> logger,
                [AsParameters] Request request,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    ValidationResult validationResult = validator.Validate(request);

                    if (!validationResult.IsValid)
                    {
                        return Results.ValidationProblem(validationResult.ToDictionary());
                    }

                    var responses = await handler.GetFlowers(request, cancellationToken);
                    return Results.Ok(responses);
                }
                catch (Exception e)
                {
                    logger.LogError("Error occured while getting flowers: {Message}", e.Message);
                    return Results.InternalServerError();
                }
            })
            .Produces<Response[]>()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Gets all flowers.")
            .WithTags("Flowers");
    }
}