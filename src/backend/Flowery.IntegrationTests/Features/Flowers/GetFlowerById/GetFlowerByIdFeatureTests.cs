namespace Flowery.IntegrationTests.Features.Flowers.GetFlowerById;

public sealed class GetFlowerByIdFeatureTests : IClassFixture<FloweryApiFactory>
{
    private readonly HttpClient _httpClient;

    public GetFlowerByIdFeatureTests(FloweryApiFactory factory)
    {
        _httpClient = factory.GetClientByPath("flowers");
    }

    [Theory]
    [InlineData("7c77c1b8-0889-4685-9d84-6eb4500f8926")]
    [InlineData("rose")]
    public async Task GetFlowerById_WhenFlowerExists_ReturnsOk(string flowerId)
    {
        // Act
        var response = await _httpClient.GetAsync($"{flowerId}", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK,
            await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken));
    }

    [Theory]
    [InlineData("8c77c1b8-0889-4685-9d84-6eb4500f8926")]
    [InlineData("not-rose")]
    public async Task GetFlowerById_WhenFlowerDoesNotExist_ReturnsNotFound(string flowerId)
    {
        // Act
        var response = await _httpClient.GetAsync($"{flowerId}", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound,
            await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken));
    }
}