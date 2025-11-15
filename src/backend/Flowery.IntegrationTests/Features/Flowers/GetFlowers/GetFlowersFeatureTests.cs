using Flowery.WebApi.Features.Flowers.GetFlowers;

namespace Flowery.IntegrationTests.Features.Flowers.GetFlowers;

public sealed class GetFlowersFeatureTests : IClassFixture<FloweryApiFactory>
{
    private readonly HttpClient _httpClient;

    public GetFlowersFeatureTests(FloweryApiFactory factory)
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
        response.StatusCode.ShouldBe(HttpStatusCode.OK,
            await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken));

        var responseBody = await response.Content.ReadFromJsonAsync<Response[]>(TestContext.Current.CancellationToken);
        responseBody.ShouldNotBeNull();
        responseBody.Length.ShouldBe(pageSize);
    }
}