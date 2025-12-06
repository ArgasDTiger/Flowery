using System.Data.Common;

namespace Flowery.Infrastructure.Data;

public interface IDbConnectionFactory
{
    Task<DbConnection> CreateConnectionAsync(CancellationToken cancellationToken);
}