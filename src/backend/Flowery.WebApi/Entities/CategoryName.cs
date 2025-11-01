using Flowery.WebApi.Shared.Enums;

namespace Flowery.WebApi.Entities;

public sealed class CategoryName
{
    public Guid CategoryId { get; init; }
    public string Name { get; set; } = null!;
    public LanguageCode LanguageCode { get; set; }
}
