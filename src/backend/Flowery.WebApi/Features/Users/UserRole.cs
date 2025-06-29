using Ardalis.SmartEnum;

namespace Flowery.WebApi.Users;

public sealed class UserRole : SmartEnum<UserRole>
{
    public static readonly UserRole User = new("User", 1);
    public static readonly UserRole Admin = new("Admin", 2);

    public UserRole(string name, int value) : base(name, value)
    {
    }
}