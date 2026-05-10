using Quiz.Application.Abstractions.Authentication;
using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Common.Results;
using Quiz.Domain.Users;

namespace Quiz.Application.UseCases.Auth.GetCurrentUser;

internal sealed class GetCurrentUserQueryHandler(
    IUserContext userContext,
    IUserRepository userRepository) : IQueryHandler<GetCurrentUserQuery, GetCurrentUserResponse>
{
    public async Task<Result<GetCurrentUserResponse>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        Guid userId = userContext.UserId;

        string role = userContext.Role;

        User? user = await userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            return Result.Failure<GetCurrentUserResponse>(UserErrors.NotFound(userId));
        }

        GetCurrentUserResponse response = new(userId, user.IdentityId, role, user.FirstName, user.Email);

        return response;
    }
}