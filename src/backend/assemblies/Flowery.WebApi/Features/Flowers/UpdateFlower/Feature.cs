using Flowery.WebApi.Shared.Extensions;
using Flowery.WebApi.Shared.Features;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Flowery.WebApi.Features.Flowers.UpdateFlower;

public sealed class UpdateFlowerFeature : IFeature
{
    public static void Register(IServiceCollection services)
    {
        services.AddSingleton<IQuery, Query>();
        services.AddScoped<IHandler, Handler>();
        services.AddSingleton<IValidator<Request>, RequestValidator>();
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("api/v1/flowers/{flowerId}", async ([FromServices] IHandler handler,
            [FromServices] IValidator<Request> validator,
            [FromServices] ILogger<UpdateFlowerFeature> logger,
            [FromRoute] string flowerId,
            [FromBody] Request request,
            CancellationToken cancellationToken) =>
        {
            try
            {
                ValidationResult validationResult = validator.Validate(request);

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                var result = await handler.UpdateFlower(flowerId, request, cancellationToken);
                return result.Match(
                    _ => Results.NoContent(),
                    _ => Results.NotFound());
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while updating flower: {Message}", ex.Message);
                return Results.InternalServerError();
            }
        });
    }
}