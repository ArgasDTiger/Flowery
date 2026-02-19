namespace Flowery.Shared.Entities;

public sealed record Image(Guid Id, string PathToSource, string CompressedPath, string ThumbnailPath)
{
    public Guid Id { get; init; }
    public string PathToSource { get; init; } = null!;
    public string CompressedPath { get; init; } = null!;
    public string ThumbnailPath { get; init; }  = null!;
}