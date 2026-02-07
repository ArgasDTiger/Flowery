using Flowery.IntegrationTests.TestHelpers;
using Flowery.IntegrationTests.TestHelpers.ApiFactories;
using Flowery.WebApi.Features.Flowers.GetFlowers;
using Flowery.WebApi.Shared.Pagination;
using SortDirection = Flowery.WebApi.Shared.Pagination.SortDirection;

namespace Flowery.IntegrationTests.Features.Flowers.GetFlowers;

[Collection(nameof(ReadonlyTestsCollection))]
public sealed class GetFlowersFeatureTests
{
    private readonly HttpClient _httpClient;

    public GetFlowersFeatureTests(ReadonlyFloweryApiFactory factory)
    {
        _httpClient = factory.GetClientByPath("flowers");
    }

    [Theory]
    [InlineData(1, 2, null, null)]
    [InlineData(1, 2, "Name", nameof(SortDirection.Asc))]
    [InlineData(1, 2, null, "1")]
    public async Task GetFlowers_WithValidParameters_ReturnsOk(int pageNumber, int pageSize, string? sortBy, string? sortDirection)
    {
        // Arrange
        var queryParams = new Dictionary<string, string?>
        {
            { "pageNumber", pageNumber.ToString() },
            { "pageSize", pageSize.ToString() }
        };

        if (sortBy is not null) queryParams.Add("sortBy", sortBy);
        if (sortDirection is not null) queryParams.Add("sortDirection", sortDirection);

        var url = QueryHelpers.AddQueryString("", queryParams);

        // Act
        var response = await _httpClient.GetAsync(url, TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeOk();
        var responseBody = await response.Content.ReadFromJsonAsync<PaginatedResponse<Response>>(TestContext.Current.CancellationToken);
        responseBody.ShouldNotBeNull();
        responseBody.Items.Length.ShouldBe(pageSize);
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(-1, 10)]
    [InlineData(1, 0)]
    [InlineData(1, -5)]
    [InlineData(1, 101)]
    public async Task GetFlowers_WithInvalidPaginationParameters_ReturnsBadRequest(int pageNumber, int pageSize)
    {
        // Arrange
        var queryParams = new Dictionary<string, string?>
        {
            { "pageNumber", pageNumber.ToString() },
            { "pageSize", pageSize.ToString() }
        };
        var url = QueryHelpers.AddQueryString("", queryParams);

        // Act
        var response = await _httpClient.GetAsync(url, TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeBadRequest();
    }

    [Theory]
    [InlineData("999")]
    [InlineData("InvalidSortDirection")]
    [InlineData("up")]
    public async Task GetFlowers_WithInvalidSortDirection_ReturnsBadRequest(string sortDirection)
    {
        // Arrange
        var queryParams = new Dictionary<string, string?>
        {
            { "pageNumber", "1" },
            { "pageSize", "10" },
            { "sortDirection", sortDirection }
        };
        var url = QueryHelpers.AddQueryString("", queryParams);

        // Act
        var response = await _httpClient.GetAsync(url, TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeBadRequest();
    }

    [Theory]
    [InlineData("999")]
    [InlineData("InvalidField")]
    [InlineData("Description")]
    public async Task GetFlowers_WithInvalidSortBy_ReturnsBadRequest(string sortBy)
    {
        // Arrange
        var queryParams = new Dictionary<string, string?>
        {
            { "pageNumber", "1" },
            { "pageSize", "10" },
            { "sortBy", sortBy }
        };
        var url = QueryHelpers.AddQueryString("", queryParams);

        // Act
        var response = await _httpClient.GetAsync(url, TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeBadRequest();
    }

    [Theory]
    [InlineData("Price", nameof(SortDirection.Asc))]
    [InlineData("Price", nameof(SortDirection.Desc))]
    [InlineData("Name", nameof(SortDirection.Asc))]
    [InlineData("Name", nameof(SortDirection.Desc))]
    public async Task GetFlowers_WithDifferentSorting_ReturnsOk(string sortBy, string sortDirection)
    {
        // Arrange
        var queryParams = new Dictionary<string, string?>
        {
            { "pageNumber", "1" },
            { "pageSize", "5" },
            { "sortBy", sortBy },
            { "sortDirection", sortDirection }
        };
        var url = QueryHelpers.AddQueryString("", queryParams);

        // Act
        var response = await _httpClient.GetAsync(url, TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeOk();
        var responseBody = await response.Content.ReadFromJsonAsync<PaginatedResponse<Response>>(TestContext.Current.CancellationToken);
        responseBody.ShouldNotBeNull();
        responseBody.Items.Length.ShouldBe(5);
    }

    [Theory]
    [InlineData("wedding-flowers")]
    [InlineData("garden-flowers")]
    public async Task GetFlowers_WithCategory_ReturnsOk(string category)
    {
        // Arrange
        var queryParams = new Dictionary<string, string?>
        {
            { "pageNumber", "1" },
            { "pageSize", "20" },
            { "category", category }
        };
        var url = QueryHelpers.AddQueryString("", queryParams);

        // Act
        var response = await _httpClient.GetAsync(url, TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeOk();
        var responseBody = await response.Content.ReadFromJsonAsync<PaginatedResponse<Response>>(TestContext.Current.CancellationToken);
        responseBody.ShouldNotBeNull();
        responseBody.Items.Length.ShouldBeGreaterThan(0);
        responseBody.Items.ShouldAllBe(f => f.Categories.Any(c => c.Slug == category));
    }

    [Fact]
    public async Task GetFlowers_WithWeddingFlowersCategory_ReturnsCorrectFlowers()
    {
        // Arrange
        var queryParams = new Dictionary<string, string?>
        {
            { "pageNumber", "1" },
            { "pageSize", "20" },
            { "category", "wedding-flowers" }
        };
        var url = QueryHelpers.AddQueryString("", queryParams);

        // Act
        var response = await _httpClient.GetAsync(url, TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeOk();
        var responseBody = await response.Content.ReadFromJsonAsync<PaginatedResponse<Response>>(TestContext.Current.CancellationToken);
        responseBody.ShouldNotBeNull();
        var flowerSlugs = responseBody.Items.Select(f => f.Slug).ToArray();
        flowerSlugs.ShouldContain("rose");
        flowerSlugs.ShouldContain("tulip-red");
        flowerSlugs.ShouldContain("orchid");
        flowerSlugs.ShouldContain("peony");
        flowerSlugs.ShouldContain("hydrangea");
    }

    [Fact]
    public async Task GetFlowers_WithNonExistingCategory_ReturnsEmptyResult()
    {
        // Arrange
        var queryParams = new Dictionary<string, string?>
        {
            { "pageNumber", "1" },
            { "pageSize", "20" },
            { "category", "some-category" }
        };
        var url = QueryHelpers.AddQueryString("", queryParams);

        // Act
        var response = await _httpClient.GetAsync(url, TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeOk();
        var responseBody = await response.Content.ReadFromJsonAsync<PaginatedResponse<Response>>(TestContext.Current.CancellationToken);
        responseBody.ShouldNotBeNull();
        responseBody.Items.Length.ShouldBe(0);
    }

    [Fact]
    public async Task GetFlowers_ShouldNotReturnDeletedFlowers()
    {
        // Arrange
        var queryParams = new Dictionary<string, string?>
        {
            { "pageNumber", "1" },
            { "pageSize", "100" }
        };
        var url = QueryHelpers.AddQueryString("", queryParams);

        // Act
        var response = await _httpClient.GetAsync(url, TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeOk();
        var responseBody = await response.Content.ReadFromJsonAsync<PaginatedResponse<Response>>(TestContext.Current.CancellationToken);
        responseBody.ShouldNotBeNull();
        responseBody.TotalCount.ShouldBe(12); // Excluding 3 deleted flowers
        responseBody.Items.ShouldAllBe(f => f.Slug != "lily" && f.Slug != "violet" && f.Slug != "lotus");
    }

    [Theory]
    [InlineData(1, 5, 5)]
    [InlineData(2, 5, 5)]
    [InlineData(4, 5, 0)]
    public async Task GetFlowers_WithPagination_ReturnsCorrectPageSize(int pageNumber, int pageSize, int expectedCount)
    {
        // Arrange
        var queryParams = new Dictionary<string, string?>
        {
            { "pageNumber", pageNumber.ToString() },
            { "pageSize", pageSize.ToString() }
        };
        var url = QueryHelpers.AddQueryString("", queryParams);

        // Act
        var response = await _httpClient.GetAsync(url, TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeOk();
        var responseBody = await response.Content.ReadFromJsonAsync<PaginatedResponse<Response>>(TestContext.Current.CancellationToken);
        responseBody.ShouldNotBeNull();
        responseBody.Items.Length.ShouldBe(expectedCount);
        responseBody.TotalCount.ShouldBe(12);
    }

    [Fact]
    public async Task GetFlowers_FlowersShouldHaveCategories()
    {
        // Arrange
        var queryParams = new Dictionary<string, string?>
        {
            { "pageNumber", "1" },
            { "pageSize", "10" }
        };
        var url = QueryHelpers.AddQueryString("", queryParams);

        // Act
        var response = await _httpClient.GetAsync(url, TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeOk();
        var responseBody = await response.Content.ReadFromJsonAsync<PaginatedResponse<Response>>(TestContext.Current.CancellationToken);
        responseBody.ShouldNotBeNull();
        
        var rose = responseBody.Items.FirstOrDefault(f => f.Slug == "rose");
        rose.ShouldNotBeNull();
        rose.Categories.Length.ShouldBeGreaterThan(0);
        rose.Categories.ShouldContain(c => c.Slug == "wedding-flowers");
    }

    [Fact]
    public async Task GetFlowers_WithSortByPrice_ShouldReturnSortedResults()
    {
        // Arrange
        var queryParams = new Dictionary<string, string?>
        {
            { "pageNumber", "1" },
            { "pageSize", "5" },
            { "sortBy", "Price" },
            { "sortDirection", nameof(SortDirection.Asc) }
        };
        var url = QueryHelpers.AddQueryString("", queryParams);

        // Act
        var response = await _httpClient.GetAsync(url, TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeOk();
        var responseBody = await response.Content.ReadFromJsonAsync<PaginatedResponse<Response>>(TestContext.Current.CancellationToken);
        responseBody.ShouldNotBeNull();
        
        for (int i = 0; i < responseBody.Items.Length - 1; i++)
        {
            responseBody.Items[i].Price.ShouldBeLessThanOrEqualTo(responseBody.Items[i + 1].Price);
        }
    }

    [Fact]
    public async Task GetFlowers_WithCategoryAndSorting_ShouldApplyBothFilters()
    {
        // Arrange
        var queryParams = new Dictionary<string, string?>
        {
            { "pageNumber", "1" },
            { "pageSize", "10" },
            { "category", "garden-flowers" },
            { "sortBy", "Price" },
            { "sortDirection", nameof(SortDirection.Desc) }
        };
        var url = QueryHelpers.AddQueryString("", queryParams);

        // Act
        var response = await _httpClient.GetAsync(url, TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeOk();
        var responseBody = await response.Content.ReadFromJsonAsync<PaginatedResponse<Response>>(TestContext.Current.CancellationToken);
        responseBody.ShouldNotBeNull();
        responseBody.Items.ShouldAllBe(f => f.Categories.Any(c => c.Slug == "garden-flowers"));
        
        for (int i = 0; i < responseBody.Items.Length - 1; i++)
        {
            responseBody.Items[i].Price.ShouldBeGreaterThanOrEqualTo(responseBody.Items[i + 1].Price);
        }
    }
}