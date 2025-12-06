using Flowery.Domain.Users;

namespace Flowery.Infrastructure.Auth.Tokens;

public readonly record struct JwtUser(string Email, UserRole Role);