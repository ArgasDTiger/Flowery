using Flowery.WebApi.Shared.Features;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Flowery.WebApi.Features.Auth.SignUp;

public sealed class SignUpFeature : IFeature
{
    public static void Register(IServiceCollection services)
    {
        services.AddSingleton<IValidator<Request>, RequestValidator>();
    }

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("api/v1/auth/signUp", async (
            [FromServices] IValidator<Request> validator,
            [FromBody] Request request, 
            CancellationToken cancellationToken) =>
        {
            
        });
    }
}