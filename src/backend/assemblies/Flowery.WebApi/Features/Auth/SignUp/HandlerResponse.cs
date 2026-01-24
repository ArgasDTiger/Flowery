using Flowery.Domain.Entities;
using Flowery.Domain.Users;

namespace Flowery.WebApi.Features.Auth.SignUp;

public sealed record HandlerResponse(string Email, UserRole Role, RefreshToken RefreshToken, string AccessToken);