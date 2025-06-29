namespace Flowery.WebApi.Flowers.Responses;

public sealed class FlowerResponse
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required string? Slug { get; init; }
}