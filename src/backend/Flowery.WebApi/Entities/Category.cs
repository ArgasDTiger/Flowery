namespace Flowery.WebApi.Entities;

public sealed class Category
{
    public int Guid { get; init; }
    public string? Slug { get; init; }
    public string Name { get; init; } = null!;
}