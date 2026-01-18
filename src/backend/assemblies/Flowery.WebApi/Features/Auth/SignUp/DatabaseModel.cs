namespace Flowery.WebApi.Features.Auth.SignUp;

public sealed record DatabaseModel(Guid Id,
    string Email,
    string PasswordHash,
    string FirstName,
    string LastName,
    string? PhoneNumber,
    string Role);