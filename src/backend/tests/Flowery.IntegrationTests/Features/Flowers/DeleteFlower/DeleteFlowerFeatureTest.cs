using Dapper;
using Flowery.Infrastructure.Data;
using Flowery.IntegrationTests.TestHelpers;
using Flowery.IntegrationTests.TestHelpers.ApiFactories;
using Flowery.IntegrationTests.TestHelpers.Seeds;
using Microsoft.Extensions.DependencyInjection;

namespace Flowery.IntegrationTests.Features.Flowers.DeleteFlower;

[Collection(nameof(WritableTestsCollection))]
public sealed class DeleteFlowerFeatureTest : IAsyncLifetime
{
    private readonly WritableFloweryApiFactory _factory;
    private readonly HttpClient _httpClient;

    public DeleteFlowerFeatureTest(WritableFloweryApiFactory factory)
    {
        _factory = factory;
        _httpClient = factory.GetClientByPath("flowers");
    }
    
    public async ValueTask InitializeAsync()
    {
        await SeedTestData();
    }

    public async ValueTask DisposeAsync()
    {
        await _factory.ResetDatabaseAsync();
    }

    [Theory]
    [InlineData("rose")]
    public async ValueTask DeleteFlower_ShouldReturnNoContent_WhenFlowerExists(string flowerId)
    {
        // Act
        var response = await _httpClient.DeleteAsync($"{flowerId}", TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeNoContent();
    }
    
    [Theory]
    [InlineData("32321")]
    public async ValueTask DeleteFlower_ShouldReturnNotFound_WhenFlowerDoesNotExist(string flowerId)
    {
        // Act
        var response = await _httpClient.DeleteAsync($"{flowerId}", TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeNotFound();
    }
    
    private async ValueTask SeedTestData()
    {
        var dbConnectionFactory = _factory.Services.GetRequiredService<IDbConnectionFactory>();
        await using var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);
        await connection.ExecuteAsync(SeedFlowers.SeedFlowersSql);
    }
}