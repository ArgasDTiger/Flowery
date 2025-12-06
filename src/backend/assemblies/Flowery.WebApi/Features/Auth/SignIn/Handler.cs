using Flowery.WebApi.Infrastructure.Auth.Passwords;
using Flowery.WebApi.Shared.ActionResults.Static;

namespace Flowery.WebApi.Features.Auth.SignIn;

public sealed class Handler : IHandler
{
    private readonly IUserPasswordHasher _passwordHasher;
    private readonly IQuery _query;

    public Handler(IUserPasswordHasher passwordHasher, IQuery query)
    {
        _passwordHasher = passwordHasher;
        _query = query;
    }

    public async Task<OneOf<Success, InvalidCredentials, NotFound>> SignInUser(Request request,
        CancellationToken cancellationToken)
    {
        var queryResult = await _query.GetUserPasswordHashByEmail(request.Email, cancellationToken);
        return queryResult.Match<OneOf<Success, InvalidCredentials, NotFound>>(
            passwordHash =>
            {
                var passwordsMatch = _passwordHasher.VerifyPassword(passwordHash, request.Password);
                if (passwordsMatch)
                {
                    return StaticResults.Success;
                }

                return StaticResults.InvalidCredentials;
            },
            notFound => notFound
        );
    }
}