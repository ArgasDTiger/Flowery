using System.Text.RegularExpressions;

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

    [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", RegexOptions.Compiled)]
    private static partial Regex PasswordRegex();
}