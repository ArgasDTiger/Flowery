using Flowery.Shared.Entities;

namespace Flowery.Infrastructure.Auth.Tokens;

public interface IRefreshTokenRepository
{
    Task InsertRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken);
    Task RevokeRefreshToken(string token, CancellationToken cancellationToken);
}