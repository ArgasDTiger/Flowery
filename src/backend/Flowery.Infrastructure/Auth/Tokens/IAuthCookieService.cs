using Flowery.Domain.Entities;

namespace Flowery.Infrastructure.Auth.Tokens;

public interface IAuthCookieService
{
    void SetAccessToken(string token);
    void SetRefreshToken(RefreshToken token);
}