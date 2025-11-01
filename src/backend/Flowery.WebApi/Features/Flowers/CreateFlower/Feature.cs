using Flowery.WebApi.Shared.Extensions;
using Flowery.WebApi.Shared.Features;
using FluentValidation;
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
        endpoints.MapPost("api/v1/flowers", async ([FromServices] IHandler handler, [FromBody] Request request, CancellationToken cancellationToken) =>
        {
            var result = await handler.CreateFlower(request, cancellationToken);
            return result.Match(
                _ => Results.Created(),
                errors => Results.ValidationProblem(errors.ToValidationProblemDictionary()));
        });
    }
}