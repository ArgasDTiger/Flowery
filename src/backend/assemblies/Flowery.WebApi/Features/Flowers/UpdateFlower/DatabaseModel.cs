using Flowery.Domain.Entities;

namespace Flowery.WebApi.Features.Flowers.UpdateFlower;

public sealed record DatabaseModel
{
    public DatabaseModel(Request request, string slug, Guid id)
    {
        Id = id;
        Slug = slug;
        Price = request.Price;
        Description = request.Description;
        FlowerNames =
        [
            ..request.FlowerNames
                .AsValueEnumerable().Select(fn => new FlowerName
                {
                    LanguageCode = fn.LanguageCode,
                    Name = fn.Name,
                })
        ];
    }
    public Guid? Id { get; }
    public decimal Price { get; }
    public string Description { get; }
    public string Slug { get; }
    public ImmutableArray<FlowerName> FlowerNames { get; }
}