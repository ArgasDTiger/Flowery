namespace Flowery.WebApi.Features.Auth.SignUp;

public interface IQuery
{
    Task<bool> UserWithEmailExists(string email, CancellationToken cancellationToken);
}