using Microsoft.AspNetCore.Identity;
using Quiz.Application.Abstractions.Authentication;
using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Common.Results;
using Quiz.Domain.Identities;
using Quiz.Domain.Roles;
using Quiz.Domain.Users;

namespace Quiz.Application.UseCases.Auth.Register;

internal sealed class RegisterCommandHandler(
    IUserRepository userRepository,
    IUserRoleRepository userRoleRepository,
    IIdentityProviderService identityProviderService,
    IUnitOfWork unitOfWork) : ICommandHandler<RegisterCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        bool emailTaken = await userRepository.ExistsByEmailAsync(command.Email, cancellationToken);
        if (emailTaken)
            return Result.Failure<Guid>(UserErrors.EmailAlreadyInUse(command.Email));

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

        Result<string> identityResult = await identityProviderService.RegisterAsync(
            command.Email, command.Password, cancellationToken);

        if (identityResult.IsFailure)
        {
            await transaction.RollbackAsync();
            return Result.Failure<Guid>(identityResult.Error);
        }

        Result<User> userResult = User.Create(
            command.FirstName, command.LastName, command.Email, identityResult.Value);

        if (userResult.IsFailure)
        {
            await transaction.RollbackAsync();
            return Result.Failure<Guid>(userResult.Error);
        }

        await userRepository.AddAsync(userResult.Value, cancellationToken);
        await userRoleRepository.AddAsync(
            new UserRole { UserId = userResult.Value.Id, RoleName = Role.Names.User },
            cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return Result.Success(userResult.Value.Id);
    }
}