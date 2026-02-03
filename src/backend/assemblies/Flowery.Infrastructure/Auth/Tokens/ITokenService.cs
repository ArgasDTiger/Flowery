using Flowery.Shared.Entities;

namespace Flowery.Infrastructure.Auth.Tokens;

public interface ITokenService
{
    string GenerateJwtToken(JwtUser user);
    RefreshToken GenerateRefreshToken(Guid userId);
}