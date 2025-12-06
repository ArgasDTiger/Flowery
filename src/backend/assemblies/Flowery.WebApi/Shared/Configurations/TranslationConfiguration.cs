using System.ComponentModel.DataAnnotations;
using Flowery.Domain.Enums;

namespace Flowery.WebApi.Shared.Configurations;

public sealed record TranslationConfiguration
{
    [Required]
    public LanguageCode SlugDefaultLanguage { get; init; }
}