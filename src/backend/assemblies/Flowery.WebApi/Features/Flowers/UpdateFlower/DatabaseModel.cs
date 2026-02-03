using Flowery.Shared.Entities;

namespace Flowery.WebApi.Features.Flowers.UpdateFlower;

public sealed record DatabaseModel
{
    public static DatabaseModel CreateWithSlugId(Request request, string oldSlug, string newSlug)
    {
        ArgumentNullException.ThrowIfNull(oldSlug);
        return new DatabaseModel
        {
            Price = request.Price,
            Description = request.Description,
            FlowerNames = GetFlowerNames(request),
            OriginalSlug = oldSlug,
            NewSlug = newSlug
        };
    }

    public static DatabaseModel CreateWithGuidId(Request request, string newSlug, Guid id)
    {
        return new DatabaseModel
        {
            Id = id,
            Price = request.Price,
            Description = request.Description,
            FlowerNames = GetFlowerNames(request),
            NewSlug = newSlug
        };
    }

    public Guid? Id { get; private init; }
    public string? OriginalSlug { get; private init; }
    public decimal Price { get; private init; }
    public string Description { get; private init; } = null!;
    public string NewSlug { get; private init; } = null!;
    public ImmutableArray<FlowerName> FlowerNames { get; private init; }
    public bool IsIdGuid => Id is not null;
    public bool IsSlugChanged => !string.Equals(OriginalSlug, NewSlug, StringComparison.OrdinalIgnoreCase);

    private static ImmutableArray<FlowerName> GetFlowerNames(Request request)
    {
        return
        [
            ..request.FlowerNames
                .AsValueEnumerable().Select(fn => new FlowerName
                {
                    LanguageCode = fn.LanguageCode,
                    Name = fn.Name,
                })
        ];
    }
}