using System.ComponentModel.DataAnnotations;
using Flowery.WebApi.Shared.Enums;

namespace Flowery.WebApi.Shared.Settings;

public sealed record TranslationSettings
{
    [Required]
    public LanguageCode SlugDefaultLanguage { get; init; }
}