using Flowery.WebApi.Entities;

namespace Flowery.WebApi.Infrastructure.Auth.Tokens;

internal interface IAuthCookieService
{
    void SetAccessToken(string token);
    void SetRefreshToken(RefreshToken token);
}