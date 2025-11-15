namespace Flowery.IntegrationTests.TestHelpers;

public static class ShouldlyExtensions
{
    public static void ShouldBeOk(this HttpResponseMessage httpResponse) =>
        httpResponse.StatusCode.ShouldBe(HttpStatusCode.OK,
            httpResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).GetAwaiter().GetResult());

    public static void ShouldBeCreated(this HttpResponseMessage httpResponse) =>
        httpResponse.StatusCode.ShouldBe(HttpStatusCode.Created,
            httpResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).GetAwaiter().GetResult());
    
    public static void ShouldBeNoContent(this HttpResponseMessage httpResponse) =>
        httpResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent,
            httpResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).GetAwaiter().GetResult());
    
    public static void ShouldBeBadRequest(this HttpResponseMessage httpResponse) =>
        httpResponse.StatusCode.ShouldBe(HttpStatusCode.BadRequest,
            httpResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).GetAwaiter().GetResult());

    public static void ShouldBeNotFound(this HttpResponseMessage httpResponse) =>
        httpResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound,
            httpResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).GetAwaiter().GetResult());
}