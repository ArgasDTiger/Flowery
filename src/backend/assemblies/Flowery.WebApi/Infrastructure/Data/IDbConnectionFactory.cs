using System.Data.Common;

namespace Flowery.WebApi.Infrastructure.Data;

public interface IDbConnectionFactory
{
    Task<DbConnection> CreateConnectionAsync(CancellationToken cancellationToken);
}