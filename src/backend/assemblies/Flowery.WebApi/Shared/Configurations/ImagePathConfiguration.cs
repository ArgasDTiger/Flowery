using System.ComponentModel.DataAnnotations;

namespace Flowery.WebApi.Shared.Configurations;

public sealed record ImagePathConfiguration
{
    [Required]
    [MinLength(15, ErrorMessage = "Image path must be at least 15 characters long.")]
    public string DefaultImagePath { get; init; } = null!;
}