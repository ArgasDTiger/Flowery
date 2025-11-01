namespace Flowery.WebApi.Features.Flowers.GetFlowerById;

public sealed record Response(Guid Id, string Name, string Slug, decimal Price);