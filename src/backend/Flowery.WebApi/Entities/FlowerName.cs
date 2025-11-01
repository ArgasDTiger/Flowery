using Flowery.WebApi.Shared.Enums;

namespace Flowery.WebApi.Entities;

public sealed class FlowerName
{
    public Guid FlowerId { get; init; }
    public LanguageCode LanguageCode { get; set; }
    public string Name { get; set; } = null!;
}