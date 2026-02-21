using Flowery.WebApi.Shared.Features;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Flowery.WebApi.Features.Categories.UpdateCategory;

public sealed class UpdateCategoryFeature : IFeature
{
    public static void Register(IServiceCollection services)
    {
        services.AddSingleton<IQuery, Query>();
        services.AddScoped<IHandler, Handler>();
        services.AddSingleton<IValidator<Request>, RequestValidator>();
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("api/v1/category/{slug}", async (
                [FromServices] IHandler handler,
                [FromServices] IValidator<Request> validator,
                [FromServices] ILogger<UpdateCategoryFeature> logger,
                [FromRoute] string slug,
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

                    var result = await handler.UpdateCategory(slug, request, cancellationToken);
                    return result.Match(
                        _ => Results.NoContent(),
                        _ => Results.NotFound());
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error occured while updating category: {Message}", ex.Message);
                    return Results.InternalServerError();
                }
            })
            .DisableAntiforgery()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Updates an existing category.")
            .WithTags("Categories");
    }
}