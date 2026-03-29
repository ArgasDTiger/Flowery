namespace Flowery.Shared.Entities;

public sealed record Image(Guid Id, string PathToSource, string CompressedPath, string? ThumbnailPath)
{
    public Image(Guid Id, string PathToSource) : this(Id, PathToSource, PathToSource, null)
    {
    }
};