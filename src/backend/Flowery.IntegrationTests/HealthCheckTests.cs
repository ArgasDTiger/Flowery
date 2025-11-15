using Flowery.IntegrationTests.TestHelpers;

namespace Flowery.IntegrationTests;

public sealed class HealthCheckTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;

    public HealthCheckTests(WebApplicationFactory<Program> factory)
    {
        _httpClient = factory.CreateDefaultClient();
    }

    [Fact]
    public async Task HealthCheck_ReturnsSuccess()
    {
        // Act
        var response = await _httpClient.GetAsync("/health", TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeOk();
    }
}