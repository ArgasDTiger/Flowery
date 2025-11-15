using Flowery.IntegrationTests.TestHelpers;
using Flowery.IntegrationTests.TestHelpers.ApiFactories;
using Flowery.WebApi.Features.Flowers.GetFlowerById;
using Microsoft.Extensions.DependencyInjection;

namespace Flowery.IntegrationTests.Features.Flowers.GetFlowerById;

[Collection(nameof(ReadonlyTestsCollection))]
public sealed class QueryTests
{
    private readonly ReadonlyFloweryApiFactory _factory;

    public QueryTests(ReadonlyFloweryApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetFlowerById_ShouldReturnFlower_WhenFlowerExists()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();
        var guid = Guid.Parse("7c77c1b8-0889-4685-9d84-6eb4500f8926");

        // Act
        var result =
            await query.GetFlowerById(guid, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(guid);
        result.Name.ShouldBe("Роза");
    }

    [Fact]
    public async Task GetFlowerById_ShouldReturnNull_WhenFlowerDoesNotExists()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();
        var guid = Guid.Parse("8c77c1b8-0889-4685-9d84-6eb4500f8926");

        // Act
        var result =
            await query.GetFlowerById(guid, CancellationToken.None);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public async Task GetFlowerBySlug_ShouldReturnFlower_WhenFlowerExists()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();

        // Act
        var result = await query.GetFlowerBySlug("rose", CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Name.ShouldBe("Роза");
    }

    [Fact]
    public async Task GetFlowerBySlug_ShouldReturnNull_WhenFlowerDoesNotExists()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();

        // Act
        var result = await query.GetFlowerBySlug("111111", CancellationToken.None);

        // Assert
        result.ShouldBeNull();
    }
}