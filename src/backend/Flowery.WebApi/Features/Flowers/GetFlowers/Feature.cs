using Flowery.WebApi.Shared.Extensions;
using Flowery.WebApi.Shared.Features;
using FluentValidation;
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
                async ([FromServices] IHandler handler, [AsParameters] Request request,
                    CancellationToken cancellationToken) =>
                {
                    var result = await handler.GetFlowers(request, cancellationToken);

                    return result.Match(
                        flowers => Results.Ok(flowers),
                        errors => Results.ValidationProblem(errors.ToValidationProblemDictionary()));
                })
            // .RequireAuthorization()
            ;
    }
}