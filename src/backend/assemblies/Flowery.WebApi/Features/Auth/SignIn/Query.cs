using Dapper;
using Flowery.Domain.ActionResults;
using Flowery.Domain.ActionResults.Static;
using Flowery.Infrastructure.Data;

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

    private const string GetUserDataByEmailSql = "SELECT email, role, passwordhash FROM Users WHERE Email = @Email LIMIT 1;";
}