using Flowery.WebApi.Entities;

namespace Flowery.WebApi.Features.Flowers.CreateFlower;

public sealed class DatabaseModel
{
    public decimal Price { get; init; }
    public string Description { get; init; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public IReadOnlyList<FlowerName> FlowerNames { get; init; } = null!;

    public static DatabaseModel FromRequest(Request request)
    {
        return new DatabaseModel
        {
            Price = request.Price,
            Description = request.Description,
            FlowerNames = request.FlowerNames
                .Select(fn => new FlowerName
                {
                    LanguageCode = fn.LanguageCode,
                    Name = fn.Name,
                }).ToArray()
        };
    }
}