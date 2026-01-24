using Flowery.Domain.ActionResults;

namespace Flowery.WebApi.Features.Auth.SignUp;

public interface IHandler
{
    Task<OneOf<Success, Error>> SignUpUser(Request request, CancellationToken cancellationToken);
}