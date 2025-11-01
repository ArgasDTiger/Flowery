using System.Collections.Immutable;
using Flowery.WebApi.Entities;

namespace Flowery.WebApi.Features.Flowers.CreateFlower;

public sealed record DatabaseModel
{
    public decimal Price { get; private init; }
    public string Description { get; private init; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public ImmutableArray<FlowerName> FlowerNames { get; private init; }

    public static DatabaseModel FromRequest(Request request)
    {
        return new DatabaseModel
        {
            Price = request.Price,
            Description = request.Description,
            FlowerNames = [
                ..request.FlowerNames
                    .Select(fn => new FlowerName
                    {
                        LanguageCode = fn.LanguageCode,
                        Name = fn.Name,
                    })
            ]
        };
    }
}