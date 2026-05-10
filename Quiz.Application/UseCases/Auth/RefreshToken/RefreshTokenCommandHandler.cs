using Quiz.Application.Abstractions.Authentication;
using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Application.Models;
using Quiz.Application.UseCases.Auth.Login;
using Quiz.Common.Results;
using Quiz.Domain.Users;

namespace Quiz.Application.UseCases.Auth.RefreshToken;

internal sealed class RefreshTokenCommandHandler(
    IUserRepository userRepository,
    IUserRoleRepository userRoleRepository,
    ITokenManager tokenManager) : ICommandHandler<RefreshTokenCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByRefreshTokenAsync(command.RefreshToken, cancellationToken);
        if (user is null || user.IsDeleted)
        {
            return Result.Failure<LoginResponse>(UserErrors.InvalidRefreshToken());
        }

        if (!user.IsActive)
        {
            return Result.Failure<LoginResponse>(UserErrors.Inactive(user.Id));
        }

        string? storedToken = await tokenManager.GetAuthenticationTokenAsync(user.IdentityId, cancellationToken);
        if (storedToken != command.RefreshToken)
        {
            return Result.Failure<LoginResponse>(UserErrors.InvalidRefreshToken());
        }

        string? roleName = await userRoleRepository.GetRoleNameByUserIdAsync(user.Id, cancellationToken);
        if (roleName is null)
        {
            return Result.Failure<LoginResponse>(UserErrors.RoleNotAssigned(user.Id));
        }

        TokenResponse tokenResponse = await tokenManager.GenerateTokensAsync(user.Id, user.IdentityId, roleName, cancellationToken);

        return Result.Success(new LoginResponse(tokenResponse.AccessToken, tokenResponse.RefreshToken));
    }
}