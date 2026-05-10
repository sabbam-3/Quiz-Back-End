using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Common.Results;
using Quiz.Domain.Users;

namespace Quiz.Application.UseCases.Users.Update;

internal sealed class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(command.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound(command.UserId));
        }

        if (!string.Equals(user.Email, command.Email, StringComparison.OrdinalIgnoreCase))
        {
            bool emailTaken = await userRepository.ExistsByEmailAsync(command.Email, cancellationToken);
            if (emailTaken)
            {
                return Result.Failure(UserErrors.EmailAlreadyInUse(command.Email));
            }
        }

        user.Update(command.FirstName, command.LastName, command.Email);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}