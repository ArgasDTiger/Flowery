using Flowery.WebApi.Shared.Extensions;

namespace Flowery.WebApi.UnitTests.Shared.Extensions;

public sealed class ValidationExtensionsTests
{
    [Theory]
    [InlineData("a@a.com", true)]
    [InlineData("fff@f.com", true)]
    [InlineData("fff", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValidEmail_ShouldReturn_ExpectedResult(string? email, bool isValid)
    {
        var result = email!.IsValidEmail();
        Assert.Equal(isValid, result);
    }
}