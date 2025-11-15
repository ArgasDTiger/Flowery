using System.Collections;
using Flowery.WebApi.Shared.Extensions;
using Flowery.WebApi.Shared.Pagination;

namespace Flowery.WebApi.UnitTests.Shared.Extensions;

public sealed class DatabaseQueryExtensionsTests
{
    [Theory]
    [InlineData(SortDirection.Ascending, "ASC")]
    [InlineData(SortDirection.Descending, "DESC")]
    public void ToSqlOrderDirection_ShouldReturnCorrectDirection(SortDirection sortDirection, string expectedResult)
    {
        var result = sortDirection.ToSqlOrderDirection();
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [ClassData(typeof(PaginationParamsData))]
    public void GetSqlOffset_ShouldReturnCorrectOffset(PaginationParams paginationParams, int expectedResult)
    {
        var result = paginationParams.GetSqlOffset();
        Assert.Equal(expectedResult, result);
    }

    private sealed record TestablePaginationParams(int PageNumber, int PageSize)
        : PaginationParams(PageNumber, PageSize);

    private sealed class PaginationParamsData : IEnumerable<TheoryDataRow<TestablePaginationParams, int>>
    {
        public IEnumerator<TheoryDataRow<TestablePaginationParams, int>> GetEnumerator()
        {
            yield return new TheoryDataRow<TestablePaginationParams, int>(new TestablePaginationParams(1, 5), 0);
            yield return new TheoryDataRow<TestablePaginationParams, int>(new TestablePaginationParams(5, 10), 40);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}