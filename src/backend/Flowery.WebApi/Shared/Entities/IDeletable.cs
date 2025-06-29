namespace Flowery.WebApi.Shared.Entities;

public interface IDeletable
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}