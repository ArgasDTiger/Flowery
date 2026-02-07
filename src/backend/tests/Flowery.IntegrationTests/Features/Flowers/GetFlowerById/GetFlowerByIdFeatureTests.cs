using Flowery.IntegrationTests.TestHelpers;
using Flowery.IntegrationTests.TestHelpers.ApiFactories;

namespace Flowery.IntegrationTests.Features.Flowers.GetFlowerById;

[Collection(nameof(ReadonlyTestsCollection))]
public sealed class GetFlowerByIdFeatureTests
{
    private readonly HttpClient _httpClient;

    public GetFlowerByIdFeatureTests(ReadonlyFloweryApiFactory factory)
    {
        _httpClient = factory.GetClientByPath("flowers");
    }

    [Theory]
    [InlineData("rose")]
    public async Task GetFlowerById_WhenFlowerExists_ReturnsOk(string flowerId)
    {
        // Act
        var response = await _httpClient.GetAsync($"{flowerId}", TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeOk();
    }

    [Theory]
    [InlineData("not-rose")]
    public async Task GetFlowerById_WhenFlowerDoesNotExist_ReturnsNotFound(string flowerId)
    {
        // Act
        var response = await _httpClient.GetAsync($"{flowerId}", TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeNotFound();
    }
}