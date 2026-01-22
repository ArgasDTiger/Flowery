using Flowery.Domain.ActionResults;

namespace Flowery.WebApi.Features.Auth.SignIn;

public interface IHandler
{
    Task<OneOf<Response, InvalidCredentials, NotFound>> SignInUser(Request request, CancellationToken cancellationToken);
}