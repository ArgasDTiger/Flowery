using Flowery.Domain.Users;

namespace Flowery.WebApi.Features.Auth.SignIn;

public sealed record DatabaseResponse(string Email, UserRole Role, string PasswordHash);