using System.Globalization;
using System.Text;

namespace Flowery.WebApi.Shared.Extensions;

public static class SlugifierExtension
{
    // TODO: pass LanguageCode to use proper remapping
    public static string GenerateSlug(this string value, int currentCount = 0)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "";
        }

        var normalisedValue = value.Normalize(NormalizationForm.FormKD);
        var sb = new StringBuilder(normalisedValue.Length);
        var prevDash = false;
        const int maxLength = 80;

        foreach (char c in normalisedValue)
        {
            var category = CharUnicodeInfo.GetUnicodeCategory(c);

            if (category == UnicodeCategory.NonSpacingMark)
            {
                continue;
            }

            switch (c)
            {
                case >= 'a' and <= 'z' or >= '0' and <= '9':
                    if (prevDash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevDash = false;
                    }

                    sb.Append(c);
                    break;
                case >= 'A' and <= 'Z':
                    if (prevDash && sb.Length > 0)
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
                        var swap = RemapUkrainianCharToAscii(c);

                        if (!string.IsNullOrEmpty(swap))
                        {
                            if (prevDash && sb.Length > 0)
                            {
                                sb.Append('-');
                                prevDash = false;
                            }

                            sb.Append(swap);
                        }
                    }

                    break;
            }

            if (sb.Length >= maxLength)
            {
                break;
            }
        }

        var slug = sb.ToString();
        return currentCount > 0
            ? (sb.Length > 0 && sb[^1] != '-' ? sb.Append('-') : sb).Append(++currentCount).ToString()
            : slug;
    }

    private static string RemapUkrainianCharToAscii(char c)
    {
        return c switch
        {
            'а' => "a",
            'б' => "b",
            'в' => "v",
            'г' => "h",
            'ґ' => "g",
            'д' => "d",
            'е' => "e",
            'є' => "ye",
            'ж' => "zh",
            'з' => "z",
            'и' => "y",
            'і' => "i",
            'ї' => "yi",
            'й' => "y",
            'к' => "k",
            'л' => "l",
            'м' => "m",
            'н' => "n",
            'о' => "o",
            'п' => "p",
            'р' => "r",
            'с' => "s",
            'т' => "t",
            'у' => "u",
            'ф' => "f",
            'х' => "kh",
            'ц' => "ts",
            'ч' => "ch",
            'ш' => "sh",
            'щ' => "shch",
            'ь' => "",
            'ю' => "yu",
            'я' => "ya",
            _ => c.ToString()
        };
    }

    private static string RemapRomanianCharToAscii(char c)
    {
        return c switch
        {
            'ă' => "a",
            'â' => "a",
            'î' => "i",
            'ș' => "s",
            'ş' => "s",
            'ț' => "t",
            'ţ' => "t",
            _ => c.ToString()
        };
    }
}
