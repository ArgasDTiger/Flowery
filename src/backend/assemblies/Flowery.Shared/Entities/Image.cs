namespace Flowery.Shared.Entities;

public sealed class Image
{
    public Guid Id { get; init; }
    public string PathToSource { get; init; } = null!;
    public string CompressedPath { get; init; } = null!;
    public string ThumbnailPath { get; init; }  = null!;
}