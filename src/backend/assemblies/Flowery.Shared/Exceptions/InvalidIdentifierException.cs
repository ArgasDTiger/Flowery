namespace Flowery.Shared.Exceptions;

public sealed class InvalidIdentifierException<T>(params string[] expectedIds)
    : ArgumentException($"Expected identifier to be one of: {string.Join(", ", expectedIds)}, but was {typeof(T).Name}");