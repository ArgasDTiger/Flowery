namespace Flowery.WebApi.Features.Auth.SignUp;

public sealed class Query : IQuery
{
    public Task<bool> UserWithEmailExists(string email, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}