using Microsoft.AspNetCore.Identity;
using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Common.Results;
using Quiz.Domain.Identities;
using Quiz.Domain.Roles;
using Quiz.Domain.Users;

namespace Quiz.Application.UseCases.Users.Create;

internal sealed class CreateUserCommandHandler(
    IUserRepository userRepository,
    IUserRoleRepository userRoleRepository,
    IUnitOfWork unitOfWork,
    UserManager<IdentityUser<string>> userManager) : ICommandHandler<CreateUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

        bool emailTaken = await userRepository.ExistsByEmailAsync(command.Email, cancellationToken);
        if (emailTaken)
        {
            return Result.Failure<Guid>(UserErrors.EmailAlreadyInUse(command.Email));
        }

        IdentityUser<string> identity = new()
        {
            Id = Guid.NewGuid().ToString(),
            Email = command.Email,
            UserName = command.Email
        };

        IdentityResult identityResult = await userManager.CreateAsync(identity, command.Password);

        if (!identityResult.Succeeded)
        {
            await transaction.RollbackAsync();
            string description = string.Join("; ", identityResult.Errors.Select(e => e.Description));
            return Result.Failure<Guid>(IdentityErrors.RegistrationFailed(description));
        }

        Result<User> user = User.Create(command.FirstName, command.LastName, command.Email, identity.Id);

        if (user.IsFailure)
        {
            return Result.Failure<Guid>(user.Error);
        }

        await userRepository.AddAsync(user.Value, cancellationToken);

        await userRoleRepository.AddAsync(
            new UserRole { UserId = user.Value.Id, RoleName = command.Role },
            cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return Result.Success(user.Value.Id);
    }
}