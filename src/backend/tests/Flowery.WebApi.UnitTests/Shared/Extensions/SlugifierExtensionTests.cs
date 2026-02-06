using Flowery.Shared.Enums;
using Flowery.WebApi.Shared.Extensions;

namespace Flowery.WebApi.UnitTests.Shared.Extensions;

public sealed class SlugifierExtensionTests
{
    [Theory]
    [ClassData(typeof(SlugTestCases))]
    public void GenerateSlug_ShouldReturnExpectedResult_WithoutPrefix(string input, LanguageCode languageCode, string expectedResult)
    {
        // Arrange & Act
        string result = input.GenerateSlug(languageCode, addPrefix: false);

        // Assert
        result.ShouldBe(expectedResult);
    }

    [Theory]
    [ClassData(typeof(SlugTestCases))]
    public void GenerateSlug_ShouldReturnExpectedResult_WithPrefix(string input, LanguageCode languageCode,
        string expectedResult)
    {
        // Arrange & Act
        string result = input.GenerateSlug(languageCode, addPrefix: true);

        // Assert
        result[..4].ShouldNotBeEmpty();
        result[5].ShouldBe('-');
        result[6..].ShouldBe(expectedResult);
    }

    private sealed class SlugTestCases : TheoryData<string, LanguageCode, string>
    {
        public SlugTestCases()
        {
            Add("Test   Rose", LanguageCode.UA, "test-rose");
            Add("Beautiful Rose", LanguageCode.UA, "beautiful-rose");
            Add("Dashe---s", LanguageCode.UA, "dashe-s");
            Add("Dashe---s123", LanguageCode.UA, "dashe-s123");
            Add("Роза", LanguageCode.UA, "roza");
            Add("Тюльпан", LanguageCode.UA, "tyulpan");
        }
    }
}