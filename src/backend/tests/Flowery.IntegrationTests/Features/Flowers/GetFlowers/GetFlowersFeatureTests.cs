using Flowery.IntegrationTests.TestHelpers;
using Flowery.IntegrationTests.TestHelpers.ApiFactories;
using Flowery.WebApi.Features.Flowers.GetFlowers;
using Flowery.WebApi.Shared.Pagination;
using SortDirection = Shouldly.SortDirection;

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
    [InlineData(1, 2, "Name", nameof(SortDirection.Ascending))]
    [InlineData(1, 2, null, "1")]
    public async Task GetFlowers_WithValidParameters_ReturnsOk(int pageNumber, int pageSize, string? sortBy,
        string? sortDirection)
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
}