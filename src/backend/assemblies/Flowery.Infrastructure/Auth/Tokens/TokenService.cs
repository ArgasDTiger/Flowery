using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Flowery.Shared.Entities;
using Flowery.Shared.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Flowery.Infrastructure.Auth.Tokens;

internal sealed class TokenService : ITokenService
{
    private readonly ITimeService _timeService;
    private readonly AuthConfiguration _configuration;

    public TokenService(IOptions<AuthConfiguration> configuration, ITimeService timeService)
    {
        _timeService = timeService;
        _configuration = configuration.Value;
    }

    public string GenerateJwtToken(JwtUser user)
    {
        Claim[] claims =
        [
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString())
        ];

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Key)),
            SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = _timeService.UtcNow.AddMinutes(_configuration.AccessTokenLifetimeInMinutes),
            SigningCredentials = credentials,
            Issuer = _configuration.Issuer,
            Audience = _configuration.Audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }

    public RefreshToken GenerateRefreshToken(Guid userId)
    {
        DateTimeOffset utcNow = _timeService.UtcNowOffset;
        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            CreatedAt = utcNow,
            ExpiresAt = utcNow.AddMinutes(_configuration.RefreshTokenLifetimeInMinutes),
            IsRevoked = false,
            UserId = userId
        };
    }
}