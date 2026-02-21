using Flowery.WebApi.Features.Flowers.Helpers;
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
                    [FromForm] Request request,
                    CancellationToken cancellationToken) =>
                {
                    try
                    {
                        ValidationResult validationResult = validator.Validate(request);

                        if (!validationResult.IsValid)
                        {
                            return Results.ValidationProblem(validationResult.ToDictionary());
                        }

                        HandlerModel handlerModel = new HandlerModel(
                            Price: request.Price,
                            Description: request.Description,
                            FlowerNames: request.FlowerNames,
                            PrimaryImage: new ImageModel(
                                ImageStream: request.PrimaryImage.OpenReadStream(),
                                Extension: Path.GetExtension(request.PrimaryImage.FileName)),
                            GalleryImages: request.GalleryImages is null
                                ? ImmutableArray<ImageModel>.Empty
                                : request.GalleryImages.AsValueEnumerable<IFormFile>().Select(file =>
                                        new ImageModel(ImageStream: file.OpenReadStream(),
                                            Extension: Path.GetExtension(file.FileName)))
                                    .ToImmutableArray());
                        string createdFlowerSlug = await handler.CreateFlower(handlerModel, cancellationToken);
                        return Results.Created(new Uri($"api/v1/flowers/{createdFlowerSlug}", UriKind.Relative),
                            createdFlowerSlug);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error occured while creating flower: {Message}", ex.Message);
                        return Results.InternalServerError();
                    }
                })
            .DisableAntiforgery()
            .Produces(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status500InternalServerError)
            .WithSummary("Creates a new flower.")
            .WithTags("Flowers");
    }
}