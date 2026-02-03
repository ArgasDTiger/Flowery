using Dapper;
using Flowery.Infrastructure.Data;
using Flowery.Shared.ActionResults;
using Flowery.Shared.ActionResults.Static;

namespace Flowery.WebApi.Features.Auth.SignIn;

public sealed class Query : IQuery
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public Query(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<OneOf<DatabaseResponse, NotFound>> GetUserDataByEmail(string email, CancellationToken cancellationToken)
    {
        await using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        var response =
            await dbConnection.QuerySingleOrDefaultAsync<DatabaseResponse>(GetUserDataByEmailSql, new { Email = email });
        return response is null ? StaticResults.NotFound : response;
    }

    private const string GetUserDataByEmailSql = "SELECT id as UserId, email, role, passwordhash FROM Users WHERE Email = @Email LIMIT 1;";
}