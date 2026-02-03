using Dapper;
using Flowery.Infrastructure.Data;
using Flowery.Shared.Entities;

namespace Flowery.Infrastructure.Auth.Tokens;

internal sealed class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public RefreshTokenRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task InsertRefreshToken(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        await using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        int rowsAffected = await dbConnection.ExecuteAsync(InsertRefreshTokenSql, refreshToken);
        if (rowsAffected == 0) throw new Exception("Failed to insert refresh token.");
    }

    public async Task RevokeRefreshToken(string token, CancellationToken cancellationToken)
    {
        await using var dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        int rowsAffected = await dbConnection.ExecuteAsync(RevokeRefreshTokenSql, token);
        if (rowsAffected == 0) throw new Exception("Failed to revoke refresh token.");
    }

    private const string InsertRefreshTokenSql = """
                                                 INSERT INTO RefreshToken (id, token, createdat, expiresat, isrevoked, userid)
                                                 VALUES (@Id, @Token, @CreatedAt, @ExpiresAt, @IsRevoked, @UserId)
                                                 """;

    private const string RevokeRefreshTokenSql = "UPDATE RefreshToken SET isrevoked = true WHERE token = @Token";
}