using Flowery.WebApi.Shared.ActionResults.Static;

namespace Flowery.WebApi.Features.Auth.SignUp;

public sealed class Handler : IHandler
{
    private readonly IQuery _query;

    public Handler(IQuery query)
    {
        _query = query;
    }

    public async Task<OneOf<Success, Error>> SignUpUser(Request request, CancellationToken cancellationToken)
    {
        if (await _query.UserWithEmailExists(request.Email, cancellationToken))
        {
            return new Error("User with this email already exists");
        }

        return StaticResults.Success;
    }
}