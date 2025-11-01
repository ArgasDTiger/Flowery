using System.ComponentModel.DataAnnotations;
using Flowery.WebApi.Shared.Enums;

namespace Flowery.WebApi.Shared.Configurations;

// TODO: seems like records do not support validation of attributes yet
public sealed record TranslationConfiguration
{
    [Required]
    public LanguageCode SlugDefaultLanguage { get; init; }
}