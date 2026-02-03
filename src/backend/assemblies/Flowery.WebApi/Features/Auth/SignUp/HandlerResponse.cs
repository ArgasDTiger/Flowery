using Flowery.Shared.Entities;
using Flowery.Shared.Users;

namespace Flowery.WebApi.Features.Auth.SignUp;

public sealed record HandlerResponse(string Email, UserRole Role, RefreshToken RefreshToken, string AccessToken);