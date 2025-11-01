using Flowery.WebApi.Shared.Extensions;
using Flowery.WebApi.Shared.Features;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Flowery.WebApi.Features.Flowers.CreateFlower;

public sealed class Feature : IFeature
{
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<IQuery, Query>();
        services.AddScoped<IHandler, Handler>();
        services.AddScoped<IValidator<Request>, RequestValidator>();
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/v1/flowers",
            async ([FromServices] IHandler handler, [FromServices] IValidator<Request> validator,
                [FromBody] Request request, CancellationToken cancellationToken) =>
            {
                ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.Errors.ToValidationProblemDictionary());
                }

                var result = await handler.CreateFlower(request, cancellationToken);
                return Results.Created(); // TODO: return location header
            });
    }
}