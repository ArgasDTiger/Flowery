namespace Flowery.WebApi.Shared.Entities;

public interface IDeletable
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
}