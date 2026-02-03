using Flowery.Shared.Users;

namespace Flowery.WebApi.Features.Auth.SignIn;

public sealed record Response(string Email, UserRole Role);