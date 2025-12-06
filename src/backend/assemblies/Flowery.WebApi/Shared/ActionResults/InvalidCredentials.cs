namespace Flowery.WebApi.Shared.ActionResults;

public sealed record InvalidCredentials(string Message = "Invalid credentials");