using Flowery.WebApi.Shared.Extensions;
using Flowery.WebApi.Shared.Pagination;
using SortDirection = Flowery.WebApi.Shared.Pagination.SortDirection;

namespace Flowery.WebApi.UnitTests.Shared.Extensions;

public sealed class DatabaseQueryExtensionsTests
{
    [Theory]
    [InlineData(SortDirection.Asc, "ASC")]
    [InlineData(SortDirection.Desc, "DESC")]
    public void ToSqlOrderDirection_ShouldReturnCorrectDirection(SortDirection sortDirection, string expectedResult)
    {
        // Arrange & Act
        var result = sortDirection.ToSqlOrderDirection();

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Theory]
    [ClassData(typeof(PaginationParamsData))]
    public void GetSqlOffset_ShouldReturnCorrectOffset(int pageNumber, int pageSize, int expectedResult)
    {
        // Arrange
        var paginationParams = new TestablePaginationParams(pageNumber, pageSize);

        // Act
        var result = paginationParams.GetSqlOffset();

        // Assert
        result.ShouldBe(expectedResult);
    }

    private sealed record TestablePaginationParams(int PageNumber, int PageSize)
        : PaginationParams(PageNumber, PageSize);

    private sealed class PaginationParamsData : TheoryData<int, int, int>
    {
        public PaginationParamsData()
        {
            Add(1, 5, 0);
            Add(5, 10, 40);
        }
    }
}