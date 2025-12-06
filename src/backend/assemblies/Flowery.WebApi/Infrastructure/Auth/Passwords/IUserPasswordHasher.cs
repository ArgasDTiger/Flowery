namespace Flowery.WebApi.Infrastructure.Auth.Passwords;

public interface IUserPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string passwordHash, string providedPassword);
}