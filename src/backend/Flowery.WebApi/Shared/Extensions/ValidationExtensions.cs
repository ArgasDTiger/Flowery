using System.Net.Mail;
using FluentValidation.Results;

namespace Flowery.WebApi.Shared.Extensions;

public static class ValidationExtensions
{
    public static bool IsValidEmail(this string email)
    {
        var trimmedEmail = email.Trim();
        return !trimmedEmail.EndsWith('.') && MailAddress.TryCreate(email, out _);
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
}