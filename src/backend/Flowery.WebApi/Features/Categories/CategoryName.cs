using Flowery.WebApi.Shared.Enums;

namespace Flowery.WebApi.Categories;

public sealed class CategoryName
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = null!;
    public LanguageCode LanguageCode { get; set; } = null!;
}