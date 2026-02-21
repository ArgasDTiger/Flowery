using Dapper;
using Flowery.Infrastructure.Data;
using Flowery.IntegrationTests.TestHelpers;
using Flowery.IntegrationTests.TestHelpers.ApiFactories;
using Flowery.IntegrationTests.TestHelpers.Seeds;
using Flowery.Shared.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Flowery.IntegrationTests.Features.Categories.CreateCategory;

[Collection(nameof(WritableTestsCollection))]
public sealed class CreateCategoryFeatureTests : IAsyncLifetime
{
    private readonly WritableFloweryApiFactory _factory;
    private readonly HttpClient _httpClient;

    public CreateCategoryFeatureTests(WritableFloweryApiFactory factory)
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
    public async Task CreateCategory_ShouldReturnCreated_WhenRequestIsValid()
    {
        // Arrange
        var request = new
        {
            CategoryNames = new[]
            {
                new { Name = "Тропічні квіти", LanguageCode = LanguageCode.UA },
                new { Name = "Flori tropicale", LanguageCode = LanguageCode.RO }
            }
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("", request, TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeCreated();
    }

    [Fact]
    public async Task CreateCategory_ShouldReturnCreated_WhenOnlyDefaultLanguageProvided()
    {
        // Arrange
        var request = new
        {
            CategoryNames = new[]
            {
                new { Name = "Польові квіти", LanguageCode = LanguageCode.UA }
            }
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("", request, TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeCreated();
    }

    [Fact]
    public async Task CreateCategory_ShouldReturnBadRequest_WhenCategoryAlreadyExists()
    {
        // Arrange
        var request = new
        {
            CategoryNames = new[]
            {
                new { Name = "Весільні квіти", LanguageCode = LanguageCode.UA }
            }
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("", request, TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeBadRequest();
    }

    [Fact]
    public async Task CreateCategory_ShouldReturnValidationProblem_WhenCategoryNamesIsEmpty()
    {
        // Arrange
        var request = new
        {
            CategoryNames = Array.Empty<object>()
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("", request, TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeBadRequest();
    }

    [Fact]
    public async Task CreateCategory_ShouldReturnValidationProblem_WhenDefaultLanguageNameIsMissing()
    {
        // Arrange
        var request = new
        {
            CategoryNames = new[]
            {
                new { Name = "Flori tropicale", LanguageCode = LanguageCode.RO }
            }
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("", request, TestContext.Current.CancellationToken);

        // Assert
        response.ShouldBeBadRequest();
    }

    [Fact]
    public async Task CreateCategory_ShouldReturnValidationProblem_WhenNameIsEmpty()
    {
        // Arrange
        var request = new
        {
            CategoryNames = new[]
            {
                new { Name = "", LanguageCode = LanguageCode.UA }
            }
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("", request, TestContext.Current.CancellationToken);

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