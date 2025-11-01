using System.Reflection;
using Flowery.WebApi.Shared.Extensions;
using Flowery.WebApi.Shared.Pagination;
using Xunit.Sdk;

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
    [PaginationParamsData]
    public void GetSqlOffset_ShouldReturnCorrectOffset(PaginationParams paginationParams, int expectedResult)
    {
        var result = paginationParams.GetSqlOffset();
        Assert.Equal(expectedResult, result);
    }

    private sealed record TestablePaginationParams(int PageNumber, int PageSize) : PaginationParams(PageNumber, PageSize);
    
    private sealed class PaginationParamsData : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return [new TestablePaginationParams(1, 5), 0];
            yield return [new TestablePaginationParams(5, 10), 40];
        }
    }
}

