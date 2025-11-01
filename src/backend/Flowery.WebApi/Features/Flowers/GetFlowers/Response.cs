namespace Flowery.WebApi.Features.Flowers.GetFlowers;

public sealed record Response(Guid Id, string Name, string Slug, decimal Price);