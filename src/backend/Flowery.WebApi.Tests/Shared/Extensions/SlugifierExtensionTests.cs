using Flowery.WebApi.Shared.Extensions;

namespace Flowery.WebApi.Tests.Shared.Extensions;

public class SlugifierExtensionTests
{
    [Theory]
    [InlineData("Test   Rose", "test-rose")]
    [InlineData("Beautiful Rose", "beautiful-rose")]
    [InlineData("Dashe---s", "dashe-s")]
    [InlineData("Dashe---s123", "dashe-s123")]
    public void GenerateSlug_Returns_Slug(string input, string expected)
    {
        string result = input.GenerateSlug();
        Assert.Equal(expected, result);
    }
    
    [Theory]
    [InlineData("Beautiful Rose", 2, "beautiful-rose-3")]
    [InlineData("Beautiful Rose", 0, "beautiful-rose")]
    public void GenerateSlug_Returns_NumberedSlug(string input, int count, string expected)
    {
        string result = input.GenerateSlug(count);
        Assert.Equal(expected, result);
    }
}