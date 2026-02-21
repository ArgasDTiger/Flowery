using Flowery.WebApi.Shared.Features;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Flowery.WebApi.Features.Categories.CreateCategory;

public sealed class CreateCategoryFeature : IFeature
{
    public static void Register(IServiceCollection services)
    {
        services.AddSingleton<IQuery, Query>();
        services.AddScoped<IHandler, Handler>();
        services.AddSingleton<IValidator<Request>, RequestValidator>();
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/v1/category", async ([FromServices] IHandler handler,
                [FromServices] IValidator<Request> validator,
                [FromServices] ILogger<CreateCategoryFeature> logger,
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

                    var result = await handler.CreateCategory(request, cancellationToken);
                    return result.Match(
                        slug => Results.Created(new Uri($"api/v1/category/{slug}", UriKind.Relative), slug),
                        error => Results.BadRequest(error.Message));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error occured while creating category: {Message}", ex.Message);
                    return Results.InternalServerError();
                }
            })
            .Produces(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Creates a new category.")
            .WithTags("Categories");
    }
}