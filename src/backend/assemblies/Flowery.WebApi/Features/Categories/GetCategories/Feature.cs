using Flowery.Shared.Enums;
using Flowery.WebApi.Shared.Features;
using Microsoft.AspNetCore.Mvc;

namespace Flowery.WebApi.Features.Categories.GetCategories;

public sealed class GetCategoriesFeature : IFeature
{
    public static void Register(IServiceCollection services)
    {
        services.AddSingleton<IQuery, Query>();
        services.AddScoped<IHandler, Handler>();
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/v1/category",
                async ([FromServices] IHandler handler,
                    [FromServices] ILogger<GetCategoriesFeature> logger,
                    CancellationToken cancellationToken) =>
                {
                    try
                    {
                        var responses = await handler.GetCategories(LanguageCode.UA, cancellationToken);
                        return Results.Ok(responses);
                    }
                    catch (Exception e)
                    {
                        logger.LogError(e, "Error occured while getting categories: {Message}", e.Message);
                        return Results.InternalServerError();
                    }
                })
            .Produces<Response[]>()
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Gets all categories.")
            .WithTags("Categories"); 
    }
}