using Flowery.WebApi.Entities;

namespace Flowery.WebApi.Infrastructure.Auth.Tokens;

public interface IRefreshTokenRepository
{
    Task InsertRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken);
    Task RevokeRefreshToken(string token, CancellationToken cancellationToken);
}