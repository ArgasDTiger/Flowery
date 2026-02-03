using Flowery.Shared.Entities;

namespace Flowery.WebApi.Features.Auth.SignUp;

public sealed record Response(RefreshToken RefreshToken, string AccessToken);