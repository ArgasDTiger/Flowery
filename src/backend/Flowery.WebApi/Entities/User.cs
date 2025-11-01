using Flowery.WebApi.Entities.Abstractions;
using Flowery.WebApi.Features.Users;

namespace Flowery.WebApi.Entities;

public sealed class User : IDeletable
{
    public Guid Id { get; init; }
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string? PhoneNumber { get; init; }
    public string PasswordSalt { get; init; } = null!;
    public string PasswordHash { get; init; } = null!;
    public UserRole Role { get; init; }
    public bool IsEmailVerified { get; init; }
    public bool IsDeleted { get; init; }
    public DateTimeOffset? DeletedAtUtc { get; init; }
}