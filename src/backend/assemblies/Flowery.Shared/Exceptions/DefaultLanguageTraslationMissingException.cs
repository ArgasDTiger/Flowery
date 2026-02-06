using Flowery.Shared.Enums;

namespace Flowery.Shared.Exceptions;

public sealed class DefaultLanguageTranslationMissingException(LanguageCode languageCode)
    : Exception($"Missing translation for default language: {languageCode.ToString()}.");