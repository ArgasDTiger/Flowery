using Flowery.Domain.ActionResults;

namespace Flowery.WebApi.Features.Auth.SignUp;

public interface IHandler
{
    Task<OneOf<Response, Error>> SignUpUser(Request request, CancellationToken cancellationToken);
}