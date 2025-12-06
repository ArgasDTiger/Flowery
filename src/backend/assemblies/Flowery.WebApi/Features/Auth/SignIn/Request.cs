using Flowery.Domain.Users;
using Flowery.WebApi.Shared.Extensions;
using FluentValidation;

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
            .MaximumLength(UserConstants.MaxEmailLength)
            .WithMessage("Email is not valid.");
    }
}