using Flowery.Domain.Entities;
using Flowery.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Flowery.Infrastructure.Auth.Tokens;

internal sealed class AuthCookieService : IAuthCookieService
{
    private readonly ITimeService _timeService;
    private readonly AuthConfiguration _configuration;
    private readonly HttpContext _httpContext;

    public AuthCookieService(
        IHttpContextAccessor httpContextAccessor,
        ITimeService timeService,
        IOptions<AuthConfiguration> configuration)
    {
        _httpContext = httpContextAccessor.HttpContext ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _timeService = timeService;
        _configuration = configuration.Value;
    }

    public void SetAccessToken(string token)
    {
        _httpContext.Response.Cookies.Append(AuthConstants.AccessTokenCookie, token, new CookieOptions
        {
            HttpOnly = true,
            Expires = _timeService.UtcNow.AddMinutes(_configuration.AccessTokenLifetimeInMinutes),
            // TODO: Make secure
            // Secure = true
            SameSite = SameSiteMode.None,
            IsEssential = true
        });
    }

    public void SetRefreshToken(RefreshToken token)
    {
        _httpContext.Response.Cookies.Append(AuthConstants.RefreshTokenCookie, token.Token, new CookieOptions
        {
            HttpOnly = true,
            Expires = token.ExpiresAt,
            // TODO: Make secure
            // Secure = true
            SameSite = SameSiteMode.None,
            IsEssential = true
        });
    }
}