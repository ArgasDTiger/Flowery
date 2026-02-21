using Flowery.IntegrationTests.TestHelpers;
using Flowery.IntegrationTests.TestHelpers.ApiFactories;
using Flowery.Shared.Enums;
using Flowery.WebApi.Features.Categories.GetCategories;
using Microsoft.Extensions.DependencyInjection;

namespace Flowery.IntegrationTests.Features.Categories.GetCategories;

[Collection(nameof(ReadonlyTestsCollection))]
public sealed class QueryTests
{
    private readonly ReadonlyFloweryApiFactory _factory;

    public QueryTests(ReadonlyFloweryApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetCategories_ShouldReturnAllCategories()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();

        // Act
        var result = await query.GetCategories(LanguageCode.UA, CancellationToken.None);

        // Assert
        result.ShouldNotBeEmpty();
        result.Length.ShouldBe(6);
    }

    [Fact]
    public async Task GetCategories_ShouldReturnCorrectNamesAndSlugs()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();

        // Act
        var result = await query.GetCategories(LanguageCode.UA, CancellationToken.None);

        // Assert
        result.ShouldContain(r => r.Slug == "wedding-flowers" && r.Name == "Весільні квіти");
        result.ShouldContain(r => r.Slug == "garden-flowers"  && r.Name == "Садові квіти");
        result.ShouldContain(r => r.Slug == "exotic-flowers"  && r.Name == "Екзотичні квіти");
        result.ShouldContain(r => r.Slug == "spring-flowers"  && r.Name == "Весняні квіти");
        result.ShouldContain(r => r.Slug == "summer-flowers"  && r.Name == "Літні квіти");
        result.ShouldContain(r => r.Slug == "romantic-flowers" && r.Name == "Романтичні квіти");
    }
}