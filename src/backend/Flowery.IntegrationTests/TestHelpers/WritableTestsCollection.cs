using Flowery.IntegrationTests.TestHelpers.ApiFactories;

namespace Flowery.IntegrationTests.TestHelpers;

[CollectionDefinition(nameof(WritableTestsCollection))]
public sealed class WritableTestsCollection : ICollectionFixture<WritableFloweryApiFactory>;