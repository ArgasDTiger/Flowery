using Flowery.Shared.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Flowery.Infrastructure.Auth.Passwords;

internal sealed class UserPasswordHasher : IUserPasswordHasher
{
    private readonly PasswordHasher<User> _hasher;

    public UserPasswordHasher(IOptions<PasswordHasherOptions> options)
    {
        _hasher = new PasswordHasher<User>(options);
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