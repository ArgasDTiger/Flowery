using Flowery.IntegrationTests.TestHelpers;
using Flowery.IntegrationTests.TestHelpers.ApiFactories;

namespace Flowery.IntegrationTests.Features.Categories.GetCategoryById;

[Collection(nameof(ReadonlyTestsCollection))]
public sealed class GetCategoryByIdFeatureTests
{
    private readonly HttpClient _httpClient;

    public GetCategoryByIdFeatureTests(ReadonlyFloweryApiFactory factory)
    {
        _httpClient = factory.GetClientByPath("category");
    }

    [Theory]
    [InlineData("wedding-flowers")]
    public async Task GetCategoryById_WhenCategoryExists_ReturnsOk(string slug)
    {
        // Act
        var response = await _httpClient.GetAsync(slug, TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeOk();
    }

    [Theory]
    [InlineData("not-a-category")]
    public async Task GetCategoryById_WhenCategoryDoesNotExist_ReturnsNotFound(string slug)
    {
        // Act
        var response = await _httpClient.GetAsync(slug, TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeNotFound();
    }
}