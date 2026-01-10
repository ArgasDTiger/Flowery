namespace Flowery.IntegrationTests.TestHelpers;

public static class ShouldlyExtensions
{
    extension(HttpResponseMessage httpResponse)
    {
        public void ShouldBeOk() =>
            httpResponse.StatusCode.ShouldBe(HttpStatusCode.OK,
                httpResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).GetAwaiter().GetResult());

        public void ShouldBeCreated() =>
            httpResponse.StatusCode.ShouldBe(HttpStatusCode.Created,
                httpResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).GetAwaiter().GetResult());

        public void ShouldBeNoContent() =>
            httpResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent,
                httpResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).GetAwaiter().GetResult());

        public void ShouldBeBadRequest() =>
            httpResponse.StatusCode.ShouldBe(HttpStatusCode.BadRequest,
                httpResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).GetAwaiter().GetResult());

        public void ShouldBeNotFound() =>
            httpResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound,
                httpResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken).GetAwaiter().GetResult());
    }
}