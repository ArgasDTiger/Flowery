using System.Globalization;
using System.Text;
using Flowery.Shared.Enums;

namespace Flowery.WebApi.Shared.Extensions;

public static class SlugifierExtension
{
    private const int PrefixLength = 6;

    public static string GenerateSlug(this string value, LanguageCode languageCode, bool addPrefix = true)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        int prefixLength = addPrefix ? PrefixLength : 0;
        var normalisedValue = value.Normalize(NormalizationForm.FormKD);
        var sb = new StringBuilder(normalisedValue.Length + prefixLength);

        if (addPrefix)
        {
            Span<char> prefix = stackalloc char[5];
            GenerateRandomPrefix(prefix);
            sb.Append(prefix);
            sb.Append('-');
        }

        var prevDash = false;
        const int maxLength = 80;

        foreach (char c in normalisedValue)
        {
            if (sb.Length >= maxLength + prefixLength)
            {
                break;
            }

            var category = CharUnicodeInfo.GetUnicodeCategory(c);

            if (category == UnicodeCategory.NonSpacingMark)
            {
                continue;
            }

            switch (c)
            {
                case >= 'a' and <= 'z' or >= '0' and <= '9':
                    if (prevDash && sb.Length > prefixLength)
                    {
                        sb.Append('-');
                        prevDash = false;
                    }

                    sb.Append(c);
                    break;
                case >= 'A' and <= 'Z':
                    if (prevDash && sb.Length > prefixLength)
                    {
                        sb.Append('-');
                        prevDash = false;
                    }

                    sb.Append(char.ToLowerInvariant(c));
                    break;
                default:
                    if (" ,./\\-_=".Contains(c))
                    {
                        if (!prevDash && sb.Length > 0)
                        {
                            prevDash = true;
                        }
                    }
                    else
                    {
                        if (c != ' ')
                        {
                            var swap = RemapChars(c, languageCode);
                            if (prevDash && sb.Length > prefixLength)
                            {
                                sb.Append('-');
                                prevDash = false;
                            }

                            sb.Append(swap);
                        }
                    }

                    break;
            }
        }

        return sb.ToString();
    }

    private static void GenerateRandomPrefix(Span<char> prefix)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz123456789";
        for (int i = 0; i < prefix.Length; i++)
        {
            prefix[i] = chars[Random.Shared.Next(chars.Length)];
        }
    }

    private static string RemapChars(char c, LanguageCode languageCode) => languageCode switch
    {
        LanguageCode.UA => RemapUkrainianCharToAscii(c),
        LanguageCode.RO => RemapRomanianCharToAscii(c),
        _ => c.ToString()
    };

    private static string RemapUkrainianCharToAscii(char c)
    {
        return c switch
        {
            'а' or 'А' => "a",
            'б' or 'Б' => "b",
            'в' or 'В' => "v",
            'г' or 'Г' => "h",
            'ґ' or 'Ґ' => "g",
            'д' or 'Д' => "d",
            'е' or 'Е' => "e",
            'є' or 'Є' => "ye",
            'ж' or 'Ж' => "zh",
            'з' or 'З' => "z",
            'и' or 'И' => "y",
            'і' or 'І' => "i",
            'ї' or 'Ї' => "yi",
            'й' or 'Й' => "y",
            'к' or 'К' => "k",
            'л' or 'Л' => "l",
            'м' or 'М' => "m",
            'н' or 'Н' => "n",
            'о' or 'О' => "o",
            'п' or 'П' => "p",
            'р' or 'Р' => "r",
            'с' or 'С' => "s",
            'т' or 'Т' => "t",
            'у' or 'У' => "u",
            'ф' or 'Ф' => "f",
            'х' or 'Х' => "kh",
            'ц' or 'Ц' => "ts",
            'ч' or 'Ч' => "ch",
            'ш' or 'Ш' => "sh",
            'щ' or 'Щ' => "shch",
            'ь' or 'Ь' => "",
            'ю' or 'Ю' => "yu",
            'я' or 'Я' => "ya",
            _ => c.ToString()
        };
    }
    
    private static string RemapRomanianCharToAscii(char c)
    {
        return c switch
        {
            'ă' or 'Ă' => "a",
            'â' or 'Â' => "a",
            'î' or 'Î' => "i",
            'ș' or 'Ș' => "s",
            'ş' or 'Ş' => "s",
            'ț' or 'Ț' => "t",
            'ţ' or 'Ţ' => "t",
            _ => c.ToString()
        };
    }
}