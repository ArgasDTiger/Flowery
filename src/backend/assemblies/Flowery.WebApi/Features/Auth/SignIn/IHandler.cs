using Flowery.Domain.ActionResults;

namespace Flowery.WebApi.Features.Auth.SignIn;

public interface IHandler
{
    Task<OneOf<Success, InvalidCredentials, NotFound>> SignInUser(Request request, CancellationToken cancellationToken);
}