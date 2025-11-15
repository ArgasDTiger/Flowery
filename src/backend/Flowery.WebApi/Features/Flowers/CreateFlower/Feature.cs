using Flowery.WebApi.Shared.Features;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Flowery.WebApi.Features.Flowers.CreateFlower;

public sealed class CreateFlowerFeature : IFeature
{
    public static void Register(IServiceCollection services)
    {
        services.AddSingleton<IQuery, Query>();
        services.AddScoped<IHandler, Handler>();
        services.AddSingleton<IValidator<Request>, RequestValidator>();
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/v1/flowers",
            async ([FromServices] IHandler handler, 
                [FromServices] IValidator<Request> validator,
                [FromServices] ILogger<CreateFlowerFeature> logger, 
                [FromBody] Request request,
                CancellationToken cancellationToken) =>
            {
                try
                {
                    ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

                    if (!validationResult.IsValid)
                    {
                        return Results.ValidationProblem(validationResult.ToDictionary());
                    }

                    Guid createdFlowerId = await handler.CreateFlower(request, cancellationToken);
                    return Results.Created(new Uri($"api/v1/flowers/{createdFlowerId}", UriKind.Relative), createdFlowerId);
                }
                catch (Exception ex)
                {
                    logger.LogError("Error occured while creating flower: {Message}", ex.Message);
                    return Results.InternalServerError();
                }
            });
    }
}