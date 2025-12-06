using Flowery.WebApi.Shared.Extensions;
using FluentValidation;
using static Flowery.WebApi.Features.Users.Constants;

namespace Flowery.WebApi.Features.Auth.SignIn;

public sealed record Request(
    string Email,
    string Password);

public sealed class RequestValidator : AbstractValidator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.Email)
            .Must(email => email.IsValidEmail())
            .MaximumLength(MaxEmailLength)
            .WithMessage("Email is not valid.");
    }
}