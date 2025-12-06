using Flowery.Domain.ActionResults;

namespace Flowery.WebApi.Features.Auth.SignIn;

public interface IQuery
{
    Task<OneOf<string, NotFound>> GetUserPasswordHashByEmail(string email, CancellationToken cancellationToken);
}