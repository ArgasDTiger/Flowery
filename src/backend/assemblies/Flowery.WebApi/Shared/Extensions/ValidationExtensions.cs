using System.Text.RegularExpressions;
using FluentValidation.Results;

namespace Flowery.WebApi.Shared.Extensions;

public static partial class ValidationExtensions
{
    public static bool IsValidEmail(this string email)
    {
        if (string.IsNullOrEmpty(email)) return false;
        return email.Contains('@') && !email.TrimEnd().EndsWith('.') && email.Length <= 255; // TODO: find the best place to define constants
    }

    public static bool IsValidPassword(this string password)
    {
        return !string.IsNullOrWhiteSpace(password) && PasswordRegex().IsMatch(password);
    }

    public static Dictionary<string, string[]> ToValidatedDictionary(this ValidationResult validationResult) =>
        validationResult.Errors
            .AsValueEnumerable()
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.AsValueEnumerable().Select(x => x.ErrorMessage).ToArray()
            );

    [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", RegexOptions.Compiled)]
    private static partial Regex PasswordRegex();
}