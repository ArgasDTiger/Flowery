using System.ComponentModel.DataAnnotations;
using Flowery.Shared.Enums;

namespace Flowery.WebApi.Shared.Configurations;

public sealed record TranslationConfiguration
{
    [Required]
    public LanguageCode SlugDefaultLanguage { get; init; }

    public string SlugDefaultLanguageString => SlugDefaultLanguage switch
    {
        LanguageCode.UA => nameof(LanguageCode.UA),
        LanguageCode.RO => nameof(LanguageCode.RO),
        _ => throw new ArgumentOutOfRangeException()
    };
}