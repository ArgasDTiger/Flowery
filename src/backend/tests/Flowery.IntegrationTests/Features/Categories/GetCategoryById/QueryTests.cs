using Flowery.IntegrationTests.TestHelpers;
using Flowery.IntegrationTests.TestHelpers.ApiFactories;
using Flowery.WebApi.Features.Categories.GetCategoryById;
using Microsoft.Extensions.DependencyInjection;

namespace Flowery.IntegrationTests.Features.Categories.GetCategoryById;

[Collection(nameof(ReadonlyTestsCollection))]
public sealed class QueryTests
{
    private readonly ReadonlyFloweryApiFactory _factory;

    public QueryTests(ReadonlyFloweryApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetCategoryBySlug_ShouldReturnCategory_WhenCategoryExists()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();

        // Act
        var result = await query.GetCategoryBySlug("wedding-flowers", CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Slug.ShouldBe("wedding-flowers");
        result.Name.ShouldBe("Весільні квіти");
    }

    [Fact]
    public async Task GetCategoryBySlug_ShouldReturnNull_WhenCategoryDoesNotExist()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();

        // Act
        var result = await query.GetCategoryBySlug("not-a-category", CancellationToken.None);

        // Assert
        result.ShouldBeNull();
    }
}