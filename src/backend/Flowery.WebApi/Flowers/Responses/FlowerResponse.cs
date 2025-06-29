namespace Flowery.WebApi.Flowers.Responses;

public sealed class FlowerResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Slug { get; init; }
}