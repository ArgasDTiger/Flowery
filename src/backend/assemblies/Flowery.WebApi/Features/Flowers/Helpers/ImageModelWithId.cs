namespace Flowery.WebApi.Features.Flowers.Helpers;

public sealed record ImageModelWithId(Guid Id, Stream ImageStream, string Extension);