using Flowery.Domain.ActionResults;
using Flowery.Domain.ActionResults.Static;
using Flowery.Infrastructure.Auth.Passwords;

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

    public async Task<OneOf<Response, InvalidCredentials, NotFound>> SignInUser(Request request,
        CancellationToken cancellationToken)
    {
        var dbResponse = await _query.GetUserDataByEmail(request.Email, cancellationToken);
        return dbResponse.Match<OneOf<Response, InvalidCredentials, NotFound>>(
            userData =>
            {
                var passwordsMatch = _passwordHasher.VerifyPassword(userData.PasswordHash, request.Password);
                if (passwordsMatch)
                {
                    return new Response(Email: userData.Email, Role: userData.Role);
                }

                return StaticResults.InvalidCredentials;
            },
            notFound => notFound
        );
    }
}