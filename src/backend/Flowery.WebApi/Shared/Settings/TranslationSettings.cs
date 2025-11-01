using System.ComponentModel.DataAnnotations;
using Flowery.WebApi.Shared.Enums;

namespace Flowery.WebApi.Shared.Settings;

public sealed class TranslationSettings
{
    public const string SectionName = "TranslationSettings";

    [Required]
    public LanguageCode SlugDefaultLanguage { get; init; }
}