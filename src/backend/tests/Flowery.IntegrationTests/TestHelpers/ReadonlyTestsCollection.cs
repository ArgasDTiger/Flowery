using Flowery.IntegrationTests.TestHelpers.ApiFactories;

namespace Flowery.IntegrationTests.TestHelpers;

[CollectionDefinition(nameof(ReadonlyTestsCollection))]
public sealed class ReadonlyTestsCollection : ICollectionFixture<ReadonlyFloweryApiFactory>;