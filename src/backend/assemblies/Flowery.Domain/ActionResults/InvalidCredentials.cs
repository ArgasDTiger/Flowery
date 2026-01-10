namespace Flowery.Domain.ActionResults;

public sealed record InvalidCredentials(string Message = "Invalid credentials");