using Flowery.Infrastructure.Auth.Tokens;
using Flowery.WebApi.Shared.Features;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Flowery.WebApi.Features.Auth.SignUp;

public sealed class SignUpFeature : IFeature
{
    public static void Register(IServiceCollection services)
    {
        services.AddSingleton<IQuery, Query>();
        services.AddScoped<IHandler, Handler>();
        services.AddSingleton<IValidator<Request>, RequestValidator>();
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/v1/auth/signUp",
            async ([FromServices] IHandler handler,
                [FromServices] IValidator<Request> validator,
                [FromServices] ILogger<SignUpFeature> logger,
                [FromServices] IAuthCookieService cookieService,
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

                    var result = await handler.SignUpUser(request, cancellationToken);
                    return result.Match(
                        success =>
                        {
                            cookieService.SetAccessToken(success.AccessToken);
                            cookieService.SetRefreshToken(success.RefreshToken);
                            return Results.Created();
                        },
                        error => Results.BadRequest(error.Message));
                }
                catch (Exception ex)
                {
                    logger.LogError("Error occured while signing up: {Message}", ex.Message);
                    return Results.InternalServerError();
                }
            });
    }
}