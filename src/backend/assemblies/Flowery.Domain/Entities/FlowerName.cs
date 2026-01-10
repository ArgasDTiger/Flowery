using Flowery.Domain.Enums;

namespace Flowery.Domain.Entities;

public sealed class FlowerName
{
    public Guid FlowerId { get; init; }
    public LanguageCode LanguageCode { get; init; }
    public string Name { get; init; } = null!;
}