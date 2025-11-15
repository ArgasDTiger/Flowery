using Flowery.WebApi.Shared.Enums;

namespace Flowery.WebApi.Entities;

public sealed class CategoryName
{
    public Guid CategoryId { get; init; }
    public string Name { get; init; } = null!;
    public LanguageCode LanguageCode { get; init; }
}
