using Dapper;
using Flowery.Infrastructure.Data;
using Flowery.IntegrationTests.TestHelpers;
using Flowery.IntegrationTests.TestHelpers.ApiFactories;
using Flowery.IntegrationTests.TestHelpers.Seeds;
using Flowery.Shared.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Flowery.IntegrationTests.Features.Categories.UpdateCategory;

[Collection(nameof(WritableTestsCollection))]
public sealed class UpdateCategoryFeatureTests : IAsyncLifetime
{
    private readonly WritableFloweryApiFactory _factory;
    private readonly HttpClient _httpClient;

    public UpdateCategoryFeatureTests(WritableFloweryApiFactory factory)
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

    [Fact]
    public async Task UpdateCategory_ShouldReturnNoContent_WhenCategoryExists()
    {
        // Arrange
        var request = new
        {
            CategoryNames = new[]
            {
                new { Name = "Весільні квіти оновлені", LanguageCode = LanguageCode.UA },
                new { Name = "Flori de nuntă actualizate", LanguageCode = LanguageCode.RO }
            }
        };

        // Act
        var response = await _httpClient.PutAsJsonAsync(
            "wedding-flowers", request, TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeNoContent();
    }

    [Fact]
    public async Task UpdateCategory_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var request = new
        {
            CategoryNames = new[]
            {
                new { Name = "Doesn't matter", LanguageCode = LanguageCode.UA }
            }
        };

        // Act
        var response = await _httpClient.PutAsJsonAsync(
            "not-a-category", request, TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeNotFound();
    }

    [Fact]
    public async Task UpdateCategory_ShouldReturnValidationProblem_WhenRequestIsInvalid()
    {
        // Arrange
        var request = new
        {
            CategoryNames = Array.Empty<object>()
        };

        // Act
        var response = await _httpClient.PutAsJsonAsync(
            "wedding-flowers", request, TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeBadRequest();
    }

    private async ValueTask SeedTestData()
    {
        var dbConnectionFactory = _factory.Services.GetRequiredService<IDbConnectionFactory>();
        await using var connection = await dbConnectionFactory.CreateConnectionAsync(CancellationToken.None);
        await connection.ExecuteAsync(SeedCategories.SeedCategoriesSql);
    }
}