namespace Quiz.Application.UseCases.Auth.GetCurrentUser;

public sealed record GetCurrentUserResponse(Guid UserId, string IdentityId, string Role, string Name, string Email);