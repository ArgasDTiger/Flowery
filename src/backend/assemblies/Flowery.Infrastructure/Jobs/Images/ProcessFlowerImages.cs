namespace Flowery.Infrastructure.Jobs.Images;

public sealed record ProcessFlowerImages(Guid FlowerId, string FolderName, string OriginalImagePath);