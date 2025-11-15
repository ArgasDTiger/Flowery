using System.Data.Common;
using Npgsql;
using Respawn;

namespace Flowery.IntegrationTests.TestHelpers.ApiFactories;

public sealed class WritableFloweryApiFactory : BaseApiFactory
{
    private DbConnection _dbConnection = null!;
    private Respawner _respawner = null!;

    public async ValueTask ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    public override async ValueTask InitializeAsync()
    {
        await base.InitializeAsync();
        _dbConnection = new NpgsqlConnection(ConnectionString);
        await InitializeRespawner();
    }

    private async ValueTask InitializeRespawner()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"]
        });
    }

    public new async ValueTask DisposeAsync()
    {
        await _dbConnection.DisposeAsync();
        await PgContainer.StopAsync();
    }
}