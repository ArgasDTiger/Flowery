using Dapper;
using Flowery.Infrastructure.Data;

namespace Flowery.WebApi.Features.Auth.SignUp;

public sealed class Query : IQuery
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public Query(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> UserWithEmailExists(string email, CancellationToken cancellationToken)
    {
        await using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        return await dbConnection.ExecuteAsync(EmailExistsSql, new { email }) > 0;
    }

    public async Task CreateUser(DatabaseModel model, CancellationToken cancellationToken)
    {
        await using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        await dbConnection.ExecuteAsync(InsertUserSql, model);
    }

    private const string EmailExistsSql = "SELECT 1 FROM Users WHERE Email = @Email";

    private const string InsertUserSql = """
                                         INSERT INTO Users (id, email, passwordhash, firstname, lastname, phonenumber, role)
                                         VALUES (@Id, @Email, @PasswordHash, @FirstName, @LastName, @PhoneNumber, @Role)
                                         """;
}