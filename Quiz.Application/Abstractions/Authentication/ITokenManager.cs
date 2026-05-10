using Quiz.Application.Models;

namespace Quiz.Application.Abstractions.Authentication;

public interface ITokenManager
{
    Task<TokenResponse> GenerateTokensAsync(
        Guid userId,
        string identityId,
        string roleName,
        CancellationToken cancellationToken);

    Task<string?> GetAuthenticationTokenAsync(
        string identityId,
        CancellationToken cancellationToken = default);
}