using Flowery.Shared.Users;

namespace Flowery.Infrastructure.Auth.Tokens;

public readonly record struct JwtUser(string Email, UserRole Role);