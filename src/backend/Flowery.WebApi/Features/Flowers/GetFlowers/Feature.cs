using Flowery.WebApi.Shared.Extensions;
using Flowery.WebApi.Shared.Features;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Flowery.WebApi.Features.Flowers.GetFlowers;

public sealed class GetFlowersFeature : IFeature
{
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<IQuery, Query>();
        services.AddScoped<IHandler, Handler>();
        services.AddScoped<IValidator<Request>, RequestValidator>();
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/v1/flowers",
            async ([FromServices] IHandler handler, [FromServices] IValidator<Request> validator,
                [AsParameters] Request request,
                CancellationToken cancellationToken) =>
            {
                ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.Errors.ToValidationProblemDictionary());
                }

                var responses = await handler.GetFlowers(request, cancellationToken);
                return Results.Ok(responses);
            });
        // .RequireAuthorization()
        ;
    }
}