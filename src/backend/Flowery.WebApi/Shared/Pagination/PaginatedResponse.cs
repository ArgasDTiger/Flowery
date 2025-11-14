namespace Flowery.WebApi.Shared.Pagination;

public sealed record PaginatedResponse<T>
{
    public required ImmutableArray<T> Items { get; init; }

    public required int TotalCount
    {
        get;
        init
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Value cannot be negative.");
            }

            field = value;
        }
    }
}