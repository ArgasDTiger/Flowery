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

    public async Task<OneOf<string, NotFound>> GetUserPasswordHashByEmail(string email,
        CancellationToken cancellationToken)
    {
        await using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        string? passwordHash =
            await dbConnection.QuerySingleOrDefaultAsync<string>(GetUserPasswordHashByEmailSql, email);
        return passwordHash is null ? StaticResults.NotFound : passwordHash;
    }

    private const string GetUserPasswordHashByEmailSql = "SELECT passwordhash FROM Users WHERE Email = @Email LIMIT 1;";
}