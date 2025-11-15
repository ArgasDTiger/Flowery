namespace Flowery.WebApi.Features.Users;

/*public sealed class UserRole : SmartEnum<UserRole>
{
    public static readonly UserRole User = new("User", 1);
    public static readonly UserRole Admin = new("Admin", 2);

    public UserRole(string name, int value) : base(name, value)
    {
    }
}*/

public enum UserRole : byte
{
    User = 0,
    Admin = 1
}