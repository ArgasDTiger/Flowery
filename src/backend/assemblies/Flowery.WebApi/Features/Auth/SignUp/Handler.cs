using Flowery.Domain.ActionResults;
using Flowery.Domain.ActionResults.Static;
using Flowery.Domain.Users;
using Flowery.Infrastructure.Auth.Passwords;
using Flowery.Infrastructure.Auth.Tokens;

namespace Flowery.WebApi.Features.Auth.SignUp;

public sealed class Handler : IHandler
{
    private readonly IQuery _query;
    private readonly IUserPasswordHasher _passwordHasher;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenService _tokenService;

    public Handler(
        IQuery query,
        IUserPasswordHasher passwordHasher,
        IRefreshTokenRepository refreshTokenRepository,
        ITokenService tokenService)
    {
        _query = query;
        _passwordHasher = passwordHasher;
        _refreshTokenRepository = refreshTokenRepository;
        _tokenService = tokenService;
    }

    public async Task<OneOf<Success, Error>> SignUpUser(Request request, CancellationToken cancellationToken)
    {
        if (await _query.UserWithEmailExists(request.Email, cancellationToken))
        {
            return new Error("User with this email already exists");
        }

        var hashedPassword = _passwordHasher.HashPassword(request.Password);
        Guid userId = Guid.NewGuid();

        var dbModel = new DatabaseModel(
            Id: userId,
            Email: request.Email,
            PasswordHash: hashedPassword,
            FirstName: request.FirstName,
            LastName: request.LastName,
            PhoneNumber: request.PhoneNumber,
            Role: nameof(UserRole.User)
        );

        await _query.CreateUser(dbModel, cancellationToken);
        return StaticResults.Success;
    }

   
}