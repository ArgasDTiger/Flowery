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
        return await dbConnection.ExecuteScalarAsync<bool>(EmailExistsSql, new { Email = email });
    }

    public async Task CreateUser(DatabaseModel model, CancellationToken cancellationToken)
    {
        await using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        var rowsAffected = await dbConnection.ExecuteAsync(InsertUserSql, model);
        if (rowsAffected == 0) throw new Exception($"Failed to create a new user with email {model.Email}.");
    }

    private const string EmailExistsSql = "SELECT EXISTS (SELECT 1 FROM Users WHERE Email = @Email)";

    private const string InsertUserSql = """
                                         INSERT INTO Users (id, email, passwordhash, firstname, lastname, phonenumber, role)
                                         VALUES (@Id, @Email, @PasswordHash, @FirstName, @LastName, @PhoneNumber, @Role::userrole)
                                         """;
}