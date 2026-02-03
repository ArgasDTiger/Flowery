using System.ComponentModel.DataAnnotations;
using Flowery.Shared.Enums;

namespace Flowery.WebApi.Shared.Configurations;

public sealed record TranslationConfiguration
{
    [Required]
    public LanguageCode SlugDefaultLanguage { get; init; }
}