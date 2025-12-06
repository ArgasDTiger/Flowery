using Flowery.WebApi.Features.Users;

namespace Flowery.WebApi.Infrastructure.Auth.Tokens;

public readonly record struct JwtUser(string Email, UserRole Role);