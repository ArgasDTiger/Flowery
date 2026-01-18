using Flowery.WebApi.Shared.Extensions;
using FluentValidation;
using static Flowery.Domain.Users.UserConstants;

namespace Flowery.WebApi.Features.Auth.SignUp;

public sealed record Request(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string? PhoneNumber);

public sealed class RequestValidator : AbstractValidator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.Email)
            .Must(email => email.IsValidEmail())
            .WithMessage("Email is not valid.");

        RuleFor(x => x.Password)
            .Must(password => password.IsValidPassword())
            .WithMessage(
                "Password must contain at least one uppercase letter, one lowercase letter, one digit and be at least 8 characters long.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name must be provided.")
            .MaximumLength(MaxFirstNameLength).WithMessage("First name is too long.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name must be provided.")
            .MaximumLength(MaxLastNameLength).WithMessage("Last name is too long.");
        
        RuleFor(x => x.PhoneNumber)
            .MaximumLength(MaxPhoneNumberLength)
            .WithMessage("Phone number is not valid.");
    }
}