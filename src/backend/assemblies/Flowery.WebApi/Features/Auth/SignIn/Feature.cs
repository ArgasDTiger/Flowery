using Flowery.WebApi.Shared.Features;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Flowery.WebApi.Features.Auth.SignIn;

public sealed class SignInFeature : IFeature
{
    public static void Register(IServiceCollection services)
    {
        services.AddSingleton<IValidator<Request>, RequestValidator>();
        services.AddSingleton<IQuery, Query>();
        services.AddScoped<IHandler, Handler>();
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/v1/auth/signin",
                async ([FromServices] IValidator<Request> validator,
                    [FromServices] IHandler handler,
                    [FromServices] ILogger<SignInFeature> logger,
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

                        var result = await handler.SignInUser(request, cancellationToken);
                        return result.Match(
                            _ => Results.NoContent(),
                            _ => Results.Unauthorized(),
                            _ => Results.NotFound());
                    }
                    catch (Exception ex)
                    {
                        logger.LogError("Error occured while signing in: {Message}", ex.Message);
                        return Results.InternalServerError();
                    }
                })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Signs in an existing user.")
            .WithTags("Auth");
    }
}