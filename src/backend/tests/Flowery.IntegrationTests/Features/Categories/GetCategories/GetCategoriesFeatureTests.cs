using Flowery.IntegrationTests.TestHelpers;
using Flowery.IntegrationTests.TestHelpers.ApiFactories;

namespace Flowery.IntegrationTests.Features.Categories.GetCategories;

[Collection(nameof(ReadonlyTestsCollection))]
public sealed class GetCategoriesFeatureTests
{
    private readonly HttpClient _httpClient;

    public GetCategoriesFeatureTests(ReadonlyFloweryApiFactory factory)
    {
        _httpClient = factory.GetClientByPath("category");
    }

    [Fact]
    public async Task GetCategories_ShouldReturnOk()
    {
        // Act
        var response = await _httpClient.GetAsync("", TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeOk();
    }
}