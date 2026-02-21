using Dapper;
using Flowery.Infrastructure.Data;
using Flowery.IntegrationTests.TestHelpers;
using Flowery.IntegrationTests.TestHelpers.ApiFactories;
using Flowery.IntegrationTests.TestHelpers.Seeds;
using Microsoft.Extensions.DependencyInjection;

namespace Flowery.IntegrationTests.Features.Categories.DeleteCategory;

[Collection(nameof(WritableTestsCollection))]
public sealed class DeleteCategoryFeatureTests : IAsyncLifetime
{
    private readonly WritableFloweryApiFactory _factory;
    private readonly HttpClient _httpClient;

    public DeleteCategoryFeatureTests(WritableFloweryApiFactory factory)
    {
        _factory = factory;
        _httpClient = factory.GetClientByPath("category");
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
    [InlineData("wedding-flowers")]
    public async Task DeleteCategory_ShouldReturnNoContent_WhenCategoryExists(string slug)
    {
        // Act
        var response = await _httpClient.DeleteAsync(slug, TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeNoContent();
    }

    [Theory]
    [InlineData("not-a-category")]
    public async Task DeleteCategory_ShouldReturnNotFound_WhenCategoryDoesNotExist(string slug)
    {
        // Act
        var response = await _httpClient.DeleteAsync(slug, TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeNotFound();
    }

    private async ValueTask SeedTestData()
    {
        var dbConnectionFactory = _factory.Services.GetRequiredService<IDbConnectionFactory>();
        await using var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);
        await connection.ExecuteAsync(SeedCategories.SeedCategoriesSql);
    }
}