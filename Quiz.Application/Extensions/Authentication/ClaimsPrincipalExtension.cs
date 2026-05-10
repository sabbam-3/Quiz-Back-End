using System.Security.Authentication;
using System.Security.Claims;

namespace Quiz.Application.Extensions.Authentication;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
    {
        string? userId = principal?.FindFirst(CustomClaims.UserId)?.Value;

        return Guid.TryParse(userId, out Guid parsedUserId)
            ? parsedUserId
            : throw new AuthenticationException("User identifier is unavailable");
    }

    public static string GetUserRole(this ClaimsPrincipal principal)
    {
        return principal?.FindFirst(CustomClaims.Role)?.Value
            ?? throw new AuthenticationException("User role is unavailable");
    }
}