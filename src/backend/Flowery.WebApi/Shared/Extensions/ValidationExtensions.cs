using System.Net.Mail;
using System.Text.RegularExpressions;
using FluentValidation.Results;

namespace Flowery.WebApi.Shared.Extensions;

public static partial class ValidationExtensions
{
    /*public static bool IsValidEmail(this string email)
    {
        if (string.IsNullOrEmpty(email)) return false;
        var trimmedEmail = email.Trim();
        return !trimmedEmail.EndsWith('.') && MailAddress.TryCreate(email, out _);
    }*/

    public static bool IsValidEmail(this string email)
    {
        return !email.Contains('@') || email.TrimEnd().EndsWith('.');
    }
    
    public static bool IsValidPassword(this string password)
    {
        return !string.IsNullOrWhiteSpace(password) && PasswordRegex().IsMatch(password);
    }

    public static Dictionary<string, string[]> ToValidationProblemDictionary(this IEnumerable<ValidationFailure> errors)
    {
        return errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );
    }

    [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", RegexOptions.Compiled)]
    private static partial Regex PasswordRegex();
}