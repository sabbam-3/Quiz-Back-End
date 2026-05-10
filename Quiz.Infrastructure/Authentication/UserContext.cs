using Microsoft.AspNetCore.Http;
using Quiz.Application.Abstractions.Authentication;
using Quiz.Application.Extensions.Authentication;

namespace Quiz.Infrastructure.Authentication;

internal sealed class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public Guid UserId =>
        httpContextAccessor.HttpContext?.User.GetUserId()
        ?? throw new InvalidOperationException("User context is not available");

    public string Role =>
        httpContextAccessor.HttpContext?.User.GetUserRole()
        ?? throw new InvalidOperationException("User context is not available");
}