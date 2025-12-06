using Flowery.WebApi.Entities;

namespace Flowery.WebApi.Infrastructure.Auth.Tokens;

public interface ITokenService
{
    string GenerateJwtToken(JwtUser user);
    RefreshToken GenerateRefreshToken(Guid userId);
}