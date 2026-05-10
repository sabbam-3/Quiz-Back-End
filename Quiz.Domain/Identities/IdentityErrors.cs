using Quiz.Common.Results;

namespace Quiz.Domain.Identities;

public static class IdentityErrors
{
    public static Error NotFound(string identityId) => 
        Error.NotFound("Identity.NotFound", $"Identity with id: {identityId}, could not be found");

    public static Error RegistrationFailed(string description) =>
        Error.Problem("Identity.RegistrationFailed", $"User registration failed: {description}");

    public static Error PasswordChangeFailed(string description) =>
        Error.Problem("Identity.PasswordChangeFailed", $"Password change failed: {description}");

    public static Error PasswordResetFailed(string description) =>
        Error.Problem("Identity.PasswordResetFailed", $"Password reset failed: {description}");
}
