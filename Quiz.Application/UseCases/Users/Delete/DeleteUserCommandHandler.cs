using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Common.Results;
using Quiz.Domain.Users;

namespace Quiz.Application.UseCases.Users.Delete;

internal sealed class DeleteUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteUserCommand>
{
    public async Task<Result> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(command.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound(command.UserId));
        }

        if (user.IsDeleted)
        {
            return Result.Failure(UserErrors.AlreadyDeleted(command.UserId));
        }

        if (user.IsActive)
        {
            return Result.Failure(UserErrors.CannotDeleteActiveUser(command.UserId));
        }

        user.Delete();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
