using Flowery.WebApi.Shared.Enums;

namespace Flowery.WebApi.Flowers;

public sealed class FlowerName
{
    public int FlowerId { get; set; }
    public LanguageCode LanguageCode { get; set; } = null!;
    public string Name { get; set; } = null!;
}