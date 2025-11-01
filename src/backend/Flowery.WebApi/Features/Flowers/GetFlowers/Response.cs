namespace Flowery.WebApi.Features.Flowers.GetFlowers;

public sealed class Response
{
    public required Guid Id { get; init; }
    public required string Name { get; init; } = string.Empty;
    public required string Slug { get; init; } = string.Empty;
    public required decimal Price { get; init; }
}