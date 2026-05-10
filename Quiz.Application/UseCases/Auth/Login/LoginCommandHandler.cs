using Quiz.Application.Abstractions.Authentication;
using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Application.Models;
using Quiz.Common.Results;
using Quiz.Domain.Users;

namespace Quiz.Application.UseCases.Auth.Login;

internal sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IUserRoleRepository userRoleRepository,
    IIdentityProviderService identityProviderService,
    ITokenManager tokenManager) : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByEmailAsync(command.Email, cancellationToken);
        if (user is null || user.IsDeleted)
        {
            return Result.Failure<LoginResponse>(UserErrors.NotFound(command.Email));
        }

        if (!user.IsActive)
        {
            return Result.Failure<LoginResponse>(UserErrors.Inactive(user.Id));
        }

        await identityProviderService.LoginAsync(command.Email, command.Password, cancellationToken);

        string? roleName = await userRoleRepository.GetRoleNameByUserIdAsync(user.Id, cancellationToken);

        if (roleName is null)
        {
            return Result.Failure<LoginResponse>(UserErrors.RoleNotAssigned(user.Id));
        }

        TokenResponse tokenResponse = await tokenManager.GenerateTokensAsync(user.Id, user.IdentityId, roleName, cancellationToken);

        return Result.Success(new LoginResponse(tokenResponse.AccessToken, tokenResponse.RefreshToken));
    }
}