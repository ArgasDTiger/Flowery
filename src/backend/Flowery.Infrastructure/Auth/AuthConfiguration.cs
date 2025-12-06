using System.ComponentModel.DataAnnotations;

namespace Flowery.Infrastructure.Auth;

internal sealed record AuthConfiguration
{
    [Required] public string Issuer { get; init; } = null!;
    [Required] public string Audience { get; init; } = null!;
    [Required] public string Key { get; init; } = null!;
    [Required, Range(5, int.MaxValue)] public int AccessTokenLifetimeInMinutes { get; init; }
    [Required, Range(5, int.MaxValue)] public int RefreshTokenLifetimeInMinutes { get; init; }
}