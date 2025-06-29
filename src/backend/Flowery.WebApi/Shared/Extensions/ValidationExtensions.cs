using System.Net.Mail;

namespace Flowery.WebApi.Shared.Extensions;

public static class ValidationExtensions
{
    public static bool IsValidEmail(this string email)
    {
        var trimmedEmail = email.Trim();
        return !trimmedEmail.EndsWith('.') && MailAddress.TryCreate(email, out _);
    }
}