using Flowery.IntegrationTests.TestHelpers;
using Flowery.IntegrationTests.TestHelpers.ApiFactories;
using Flowery.Shared.Enums;
using Flowery.WebApi.Features.Flowers.GetFlowers;
using Microsoft.Extensions.DependencyInjection;
using SortDirection = Flowery.WebApi.Shared.Pagination.SortDirection;

namespace Flowery.IntegrationTests.Features.Flowers.GetFlowers;

[Collection(nameof(ReadonlyTestsCollection))]
public sealed class QueryTests
{
    private readonly ReadonlyFloweryApiFactory _factory;

    public QueryTests(ReadonlyFloweryApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetFlowers_ShouldReturnFlowers()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();
        var request = new Request
        {
            PageNumber = 1,
            PageSize = 3,
            SortDirection = SortDirection.Asc,
            SortBy = SortField.Name
        };

        // Act
        var result = await query.GetFlowers(request, LanguageCode.UA, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.TotalCount.ShouldBe(12);
    }

    [Fact]
    public async Task GetFlowers_SortedByPrice_Ascending_ShouldReturnCorrectOrder()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();
        var request = new Request
        {
            PageNumber = 1,
            PageSize = 3,
            SortDirection = SortDirection.Asc,
            SortBy = SortField.Price
        };

        // Act
        var result = await query.GetFlowers(request, LanguageCode.UA, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Items.Length.ShouldBe(3);
        result.Items[0].Price.ShouldBe(5.0m);
        result.Items[1].Price.ShouldBe(5.0m);
        result.Items[2].Price.ShouldBe(6.0m);
    }

    [Fact]
    public async Task GetFlowers_SortedByPrice_Descending_ShouldReturnCorrectOrder()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();
        var request = new Request
        {
            PageNumber = 1,
            PageSize = 3,
            SortDirection = SortDirection.Desc,
            SortBy = SortField.Price
        };

        // Act
        var result = await query.GetFlowers(request, LanguageCode.UA, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Items.Length.ShouldBe(3);
        result.Items[0].Price.ShouldBe(25.0m); // orchid
        result.Items[1].Price.ShouldBe(20.0m); // magnolia
        result.Items[2].Price.ShouldBe(18.0m); // hydrangea
    }

    [Fact]
    public async Task GetFlowers_SortedByName_Ascending_ShouldReturnCorrectOrder()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();
        var request = new Request
        {
            PageNumber = 1,
            PageSize = 3,
            SortDirection = SortDirection.Asc,
            SortBy = SortField.Name
        };

        // Act
        var result = await query.GetFlowers(request, LanguageCode.UA, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Items.Length.ShouldBe(3);
        result.Items.ShouldAllBe(item => !string.IsNullOrEmpty(item.Name));
    }

    [Theory]
    [InlineData("wedding-flowers")]
    [InlineData("garden-flowers")]
    public async Task GetFlowers_WithCategoryFilter_ShouldReturnOnlyFlowersInCategory(string categorySlug)
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();
        var request = new Request
        {
            PageNumber = 1,
            PageSize = 20,
            SortDirection = SortDirection.Asc,
            SortBy = SortField.Name,
            Category = categorySlug
        };

        // Act
        var result = await query.GetFlowers(request, LanguageCode.UA, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Items.Length.ShouldBeGreaterThan(0);
        result.Items.ShouldAllBe(f => f.Categories.Any(c => c.Slug == categorySlug));
    }

    [Fact]
    public async Task GetFlowers_WithWeddingFlowersCategory_ShouldReturnCorrectFlowers()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();
        var request = new Request
        {
            PageNumber = 1,
            PageSize = 20,
            SortDirection = SortDirection.Asc,
            SortBy = SortField.Name,
            Category = "wedding-flowers"
        };

        // Act
        var result = await query.GetFlowers(request, LanguageCode.UA, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        var flowerSlugs = result.Items.Select(f => f.Slug).ToArray();
        flowerSlugs.ShouldContain("rose");
        flowerSlugs.ShouldContain("tulip-red");
        flowerSlugs.ShouldContain("orchid");
        flowerSlugs.ShouldContain("peony");
        flowerSlugs.ShouldContain("hydrangea");
    }

    [Fact]
    public async Task GetFlowers_WithNotExistingCategory_ShouldReturnEmpty()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();
        var request = new Request
        {
            PageNumber = 1,
            PageSize = 20,
            SortDirection = SortDirection.Asc,
            SortBy = SortField.Name,
            Category = "some-category"
        };

        // Act
        var result = await query.GetFlowers(request, LanguageCode.UA, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Items.Length.ShouldBe(0);
    }

    [Fact]
    public async Task GetFlowers_ShouldExcludeDeletedFlowers()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();
        var request = new Request
        {
            PageNumber = 1,
            PageSize = 100,
            SortDirection = SortDirection.Asc,
            SortBy = SortField.Name
        };

        // Act
        var result = await query.GetFlowers(request, LanguageCode.UA, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.TotalCount.ShouldBe(12);
        result.Items.ShouldAllBe(f => f.Slug != "lily" && f.Slug != "violet" && f.Slug != "lotus");
    }

    [Fact]
    public async Task GetFlowers_WithUkrainianLanguage_ShouldReturnUkrainianNames()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();
        var request = new Request
        {
            PageNumber = 1,
            PageSize = 5,
            SortDirection = SortDirection.Asc,
            SortBy = SortField.Name
        };

        // Act
        var result = await query.GetFlowers(request, LanguageCode.UA, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        var carnation = result.Items.FirstOrDefault(f => f.Slug == "carnation");
        carnation.ShouldNotBeNull();
        carnation.Name.ShouldBe("Гвоздика");
    }

    [Fact]
    public async Task GetFlowers_WithRomanianLanguage_ShouldReturnRomanianNames()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();
        var request = new Request
        {
            PageNumber = 1,
            PageSize = 5,
            SortDirection = SortDirection.Asc,
            SortBy = SortField.Name
        };

        // Act
        var result = await query.GetFlowers(request, LanguageCode.RO, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        var peony = result.Items.FirstOrDefault(f => f.Slug == "peony");
        peony.ShouldNotBeNull();
        peony.Name.ShouldBe("Bujor");
    }

    [Fact]
    public async Task GetFlowers_ShouldIncludeCategories()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();
        var request = new Request
        {
            PageNumber = 1,
            PageSize = 10,
            SortDirection = SortDirection.Asc,
            SortBy = SortField.Name
        };

        // Act
        var result = await query.GetFlowers(request, LanguageCode.UA, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Items.ShouldAllBe(f => f.Categories != null);

        var rose = result.Items.FirstOrDefault(f => f.Slug == "rose");
        rose.ShouldNotBeNull();
        rose.Categories.Length.ShouldBe(3); 
    }

    [Fact]
    public async Task GetFlowers_Categories_ShouldHaveCorrectTranslations_UA()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();
        var request = new Request
        {
            PageNumber = 1,
            PageSize = 20,
            SortDirection = SortDirection.Asc,
            SortBy = SortField.Name
        };

        // Act
        var result = await query.GetFlowers(request, LanguageCode.UA, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        var rose = result.Items.FirstOrDefault(f => f.Slug == "rose");
        rose.ShouldNotBeNull();
        rose.Categories.ShouldContain(c => c.Name == "Весільні квіти");
        rose.Categories.ShouldContain(c => c.Name == "Романтичні квіти");
    }

    [Fact]
    public async Task GetFlowers_Categories_ShouldHaveCorrectTranslations_RO()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();
        var request = new Request
        {
            PageNumber = 1,
            PageSize = 20,
            SortDirection = SortDirection.Asc,
            SortBy = SortField.Name
        };

        // Act
        var result = await query.GetFlowers(request, LanguageCode.RO, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        var rose = result.Items.FirstOrDefault(f => f.Slug == "rose");
        rose.ShouldNotBeNull();
        rose.Categories.ShouldContain(c => c.Name == "Flori de nuntă");
        rose.Categories.ShouldContain(c => c.Name == "Flori romantice");
    }

    [Fact]
    public async Task GetFlowers_WithPageBeyondTotalCount_ShouldReturnNoItems()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();
        var request = new Request
        {
            PageNumber = 100,
            PageSize = 10,
            SortDirection = SortDirection.Asc,
            SortBy = SortField.Name
        };

        // Act
        var result = await query.GetFlowers(request, LanguageCode.UA, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Items.Length.ShouldBe(0);
        result.TotalCount.ShouldBe(12);
    }

    [Theory]
    [InlineData(LanguageCode.UA)]
    [InlineData(LanguageCode.RO)]
    public async Task GetFlowers_DifferentLanguages_ShouldReturnSameFlowerCount(LanguageCode languageCode)
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();
        var request = new Request
        {
            PageNumber = 1,
            PageSize = 100,
            SortDirection = SortDirection.Asc,
            SortBy = SortField.Name
        };

        // Act
        var result = await query.GetFlowers(request, languageCode, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.TotalCount.ShouldBe(12);
        result.Items.ShouldAllBe(f => !string.IsNullOrEmpty(f.Name));
    }

    [Fact]
    public async Task GetFlowers_FlowerWithMultipleCategories_ShouldReturnAllCategories()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IQuery>();
        var request = new Request
        {
            PageNumber = 1,
            PageSize = 20,
            SortDirection = SortDirection.Asc,
            SortBy = SortField.Name
        };

        // Act
        var result = await query.GetFlowers(request, LanguageCode.UA, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        var daisy = result.Items.FirstOrDefault(f => f.Slug == "daisy");
        daisy.ShouldNotBeNull();
        daisy.Categories.Length.ShouldBe(3);
        daisy.Categories.Select(c => c.Slug).ShouldContain("garden-flowers");
        daisy.Categories.Select(c => c.Slug).ShouldContain("spring-flowers");
        daisy.Categories.Select(c => c.Slug).ShouldContain("summer-flowers");
    }
}