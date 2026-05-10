using System.Security.Claims;

namespace Quiz.Application.Extensions.Authentication;

public static class CustomClaims
{
    public const string UserId = ClaimTypes.NameIdentifier;
    public const string Role = ClaimTypes.Role;
}