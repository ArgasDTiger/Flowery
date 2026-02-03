using Flowery.Shared.Users;

namespace Flowery.WebApi.Features.Auth.SignIn;

public sealed record DatabaseResponse(Guid UserId, string Email, UserRole Role, string PasswordHash);