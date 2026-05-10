namespace Quiz.Domain.Roles;

public sealed class Role
{
    public required string Name { get; init; }

    public static readonly Role Admin = new()
    {
        Name = Names.Admin
    };

    public static readonly Role User = new()
    {
        Name = Names.User
    };

    public static class Names
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
}