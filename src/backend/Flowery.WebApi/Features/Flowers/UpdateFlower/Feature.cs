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
        services.AddScoped<IQuery, Query>();
        services.AddScoped<IHandler, Handler>();
        services.AddScoped<IValidator<Request>, RequestValidator>();
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
                ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.Errors.ToValidationProblemDictionary());
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