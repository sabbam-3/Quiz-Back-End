using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Common.Results;
using Quiz.Domain.Users;

namespace Quiz.Application.UseCases.Users.Disable;

internal sealed class DisableUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DisableUserCommand>
{
    public async Task<Result> Handle(DisableUserCommand command, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(command.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound(command.UserId));
        }

        if (!user.IsActive)
        {
            return Result.Failure(UserErrors.AlreadyDisabled(command.UserId));
        }

        user.Disable();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}