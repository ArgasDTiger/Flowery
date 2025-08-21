namespace Flowery.WebApi.Shared.Pagination;

public sealed class PaginatedResponse<T>
{
    private readonly int _totalCount;
    public required List<T> Items { get; init; }

    public required int TotalCount
    {
        get => _totalCount;
        init
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Value cannot be negative.");
            }

            _totalCount = value;
        }
    }
}