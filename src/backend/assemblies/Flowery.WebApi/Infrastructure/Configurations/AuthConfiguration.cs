using System.ComponentModel.DataAnnotations;

namespace Flowery.WebApi.Infrastructure.Configurations;

internal sealed record AuthConfiguration
{
    [Required] public string Issuer { get; init; } = null!;
    [Required] public string Audience { get; init; } = null!;
    [Required] public string Key { get; init; } = null!;
    [Required, MinLength(5)] public int AccessTokenLifetimeInMinutes { get; init; }
    [Required, MinLength(5)] public int RefreshTokenLifetimeInMinutes { get; init; }
}