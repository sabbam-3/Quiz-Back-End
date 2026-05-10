namespace Quiz.Domain.Roles;

public sealed class UserRole
{
    public Guid UserId { get; init; }

    public required string RoleName { get; init; }
}