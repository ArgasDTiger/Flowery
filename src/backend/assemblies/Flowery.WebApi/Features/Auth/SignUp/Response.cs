using Flowery.WebApi.Entities;

namespace Flowery.WebApi.Features.Auth.SignUp;

public sealed record Response(RefreshToken RefreshToken, string AccessToken);