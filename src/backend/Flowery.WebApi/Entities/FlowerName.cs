using Flowery.WebApi.Shared.Enums;

namespace Flowery.WebApi.Entities;

public sealed class FlowerName
{
    public int FlowerId { get; set; }
    public LanguageCode LanguageCode { get; set; }
    public string Name { get; set; } = null!;
}