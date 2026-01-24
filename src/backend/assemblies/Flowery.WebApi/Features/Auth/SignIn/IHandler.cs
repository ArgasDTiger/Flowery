using Flowery.Domain.ActionResults;
using Flowery.WebApi.Features.Auth.SignUp;

namespace Flowery.WebApi.Features.Auth.SignIn;

public interface IHandler
{
    Task<OneOf<HandlerResponse, InvalidCredentials, NotFound>> SignInUser(Request request, CancellationToken cancellationToken);
}