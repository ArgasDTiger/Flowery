namespace Flowery.WebApi.Users;

public sealed class User
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordSalt { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public UserRole Role { get; set; } = null!;
    public bool IsEmailVerified { get; set; }
}