namespace Flowery.Shared.Exceptions;

public sealed class DiscriminatedUnionParsingException(string? message) : Exception(message);