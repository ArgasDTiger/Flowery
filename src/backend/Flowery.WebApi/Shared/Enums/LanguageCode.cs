using Ardalis.SmartEnum;

namespace Flowery.WebApi.Shared.Enums;

public sealed class LanguageCode : SmartEnum<LanguageCode>
{
    public static readonly LanguageCode Ukrainian = new LanguageCode("UA", 1);
    public static readonly LanguageCode Romanian = new LanguageCode("RO", 2);
    public LanguageCode(string name, int value) : base(name, value)
    {
    }
}