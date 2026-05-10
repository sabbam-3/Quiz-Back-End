using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Common.Results;
using Quiz.Domain.Users;

namespace Quiz.Application.UseCases.Users.GetById;

internal sealed class GetUserByIdQueryHandler(
    IUserRepository userRepository) : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(query.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFound(query.UserId));
        }

        return Result.Success(new UserResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email!,
            user.IsActive,
            user.CreatedAtUtc));
    }
}
