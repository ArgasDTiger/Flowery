﻿namespace Flowery.WebApi.Entities.Abstractions;

public interface IDeletable
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAtUtc { get; set; }
}