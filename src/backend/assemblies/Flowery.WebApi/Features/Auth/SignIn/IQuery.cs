using Flowery.Shared.ActionResults;

namespace Flowery.WebApi.Features.Auth.SignIn;

public interface IQuery
{
    Task<OneOf<DatabaseResponse, NotFound>> GetUserDataByEmail(string email, CancellationToken cancellationToken);
}