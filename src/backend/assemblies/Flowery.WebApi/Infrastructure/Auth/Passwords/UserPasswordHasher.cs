using Flowery.WebApi.Entities;
using Microsoft.AspNetCore.Identity;

namespace Flowery.WebApi.Infrastructure.Auth.Passwords;

internal sealed class UserPasswordHasher : IUserPasswordHasher
{
    private readonly PasswordHasher<User> _hasher;

    public UserPasswordHasher(PasswordHasher<User> hasher)
    {
        _hasher = hasher;
    }

    public string HashPassword(string password)
    {
        return _hasher.HashPassword(null!, password);
    }
    
    public bool VerifyPassword(string passwordHash, string providedPassword)
    {
        var result = _hasher.VerifyHashedPassword(null!, passwordHash, providedPassword);
        return result != PasswordVerificationResult.Failed;
    }
}