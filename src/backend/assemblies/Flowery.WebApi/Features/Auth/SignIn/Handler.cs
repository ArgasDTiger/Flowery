using Flowery.Domain.ActionResults;
using Flowery.Domain.ActionResults.Static;
using Flowery.Domain.Entities;
using Flowery.Domain.Exceptions;
using Flowery.Infrastructure.Auth.Passwords;
using Flowery.Infrastructure.Auth.Tokens;
using Flowery.WebApi.Features.Auth.SignUp;

namespace Flowery.WebApi.Features.Auth.SignIn;

public sealed class Handler : IHandler
{
    private readonly IUserPasswordHasher _passwordHasher;
    private readonly IQuery _query;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public Handler(
        IUserPasswordHasher passwordHasher,
        IQuery query, 
        ITokenService tokenService, 
        IRefreshTokenRepository refreshTokenRepository)
    {
        _passwordHasher = passwordHasher;
        _query = query;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<OneOf<HandlerResponse, InvalidCredentials, NotFound>> SignInUser(Request request,
        CancellationToken cancellationToken)
    {
        var dbResponse = await _query.GetUserDataByEmail(request.Email, cancellationToken);

        if (dbResponse.IsT1)
        {
            return dbResponse.AsT1;
        }

        if (!dbResponse.TryPickT0(out var userData, out _))
        {
            throw new DiscriminatedUnionParsingException("Failed to parse user data from database response while signing in.");
        }
        
        var passwordsMatch = _passwordHasher.VerifyPassword(userData.PasswordHash, request.Password);
        if (!passwordsMatch)
        {
            return StaticResults.InvalidCredentials;
        }

        var refreshToken = await GenerateAndSaveRefreshToken(userData.UserId, cancellationToken);
        var accessToken = _tokenService.GenerateJwtToken(new JwtUser(request.Email, userData.Role));
        return new HandlerResponse(Email: userData.Email, Role: userData.Role, AccessToken: accessToken, RefreshToken: refreshToken);
    }
    
    private async Task<RefreshToken> GenerateAndSaveRefreshToken(Guid userId, CancellationToken cancellationToken)
    {
        RefreshToken refreshToken = _tokenService.GenerateRefreshToken(userId);
        await _refreshTokenRepository.InsertRefreshToken(refreshToken, cancellationToken);
        return refreshToken;
    }
}