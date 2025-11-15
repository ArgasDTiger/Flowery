using Flowery.WebApi.Shared.Extensions;
using Flowery.WebApi.Shared.Pagination;
using SortDirection = Flowery.WebApi.Shared.Pagination.SortDirection;

namespace Flowery.WebApi.UnitTests.Shared.Extensions;

public sealed class FilteringExtensionsTests
{
    [Theory]
    [MemberData(nameof(ToSortDirectionEnumTestData))]
    public void ToSortDirectionEnum_ShouldReturnCorrectDirection(string? input, SortDirection expectedResult)
    {
        // Arrange & Act
        var result = input.ToSortDirectionEnum();

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("123")]
    [InlineData("SOMETHING")]
    public void ToSortDirectionEnum_ShouldReturnDefault_WhenInvalidInput(string input)
    {
        // Arrange & Act
        var result = input.ToSortDirectionEnum();

        // Assert
        result.ShouldBe(SortDirection.Ascending);
    }

    public static TheoryData<string?, SortDirection> ToSortDirectionEnumTestData()
    {
        var data = new TheoryData<string?, SortDirection>
        {
            { "asc", SortDirection.Ascending },
            { "ASC", SortDirection.Ascending },
            { "Asc", SortDirection.Ascending },
            { "ascending", SortDirection.Ascending },
            { "ASCENDING", SortDirection.Ascending },
            { "Ascending", SortDirection.Ascending },
            { "0", SortDirection.Ascending },
            { null, SortDirection.Ascending },

            { "desc", SortDirection.Descending },
            { "DESC", SortDirection.Descending },
            { "Desc", SortDirection.Descending },
            { "descending", SortDirection.Descending },
            { "DESCENDING", SortDirection.Descending },
            { "Descending", SortDirection.Descending },
            { "1", SortDirection.Descending }
        };

        return data;
    }
}