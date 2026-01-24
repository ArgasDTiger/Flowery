namespace Flowery.Domain.Exceptions;

public sealed class DiscriminatedUnionParsingException(string? message) : Exception(message);