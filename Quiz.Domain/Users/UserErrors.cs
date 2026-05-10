using Quiz.Common.Results;

namespace Quiz.Domain.Users;

public static class UserErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("Users.NotFound", $"The user with the ID '{id}' was not found");

    public static Error NotFoundByEmail(string email) =>
        Error.NotFound("Users.NotFoundByEmail", $"No user found with email '{email}'");

    public static Error EmailAlreadyInUse(string email) =>
        Error.Conflict("Users.EmailAlreadyInUse", $"The email '{email}' is already in use by another user");

    public static Error AlreadyDisabled(Guid id) =>
        Error.Failure("Users.AlreadyDisabled", $"The user with the ID '{id}' is already disabled");

    public static Error AlreadyActive(Guid id) =>
        Error.Failure("Users.AlreadyActive", $"The user with the ID '{id}' is already active");

    public static Error AlreadyDeleted(Guid id) =>
        Error.Failure("Users.AlreadyDeleted", $"The user with the ID '{id}' has already been deleted");

    public static Error CannotDeleteActiveUser(Guid id) =>
        Error.Failure("Users.CannotDeleteActiveUser", $"The user with the ID '{id}' must be disabled before deletion");

    public static Error NotFound(string email) =>
        Error.NotFound("Users.NotFound", $"No user found with email '{email}'");

    public static Error Inactive(Guid id) =>
        Error.Problem("Users.Inactive", $"The user with the ID '{id}' is inactive");

    public static Error RoleNotAssigned(Guid id) =>
        Error.Problem("Users.RoleNotAssigned", $"No role is assigned to the user with the ID '{id}'");

    public static Error InvalidCredentials() =>
        Error.Problem("Users.InvalidCredentials", "The provided email or password is incorrect");

    public static Error InvalidRefreshToken() =>
        Error.Problem("Users.InvalidRefreshToken", "The refresh token is invalid or has expired");

    public static Error InvalidResetToken() =>
        Error.Problem("Users.InvalidResetToken", "The password reset token is invalid or has expired");
}
