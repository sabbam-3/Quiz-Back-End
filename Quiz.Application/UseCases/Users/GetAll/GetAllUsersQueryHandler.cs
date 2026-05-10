using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Application.UseCases.Users.GetById;
using Quiz.Common.Results;
using Quiz.Domain.Users;

namespace Quiz.Application.UseCases.Users.GetAll;

internal sealed class GetAllUsersQueryHandler(
    IUserRepository userRepository) : IQueryHandler<GetAllUsersQuery, PagedResult<UserResponse>>
{
    public async Task<Result<PagedResult<UserResponse>>> Handle(GetAllUsersQuery query, CancellationToken cancellationToken)
    {
        PagedResult<User> paged = await userRepository.GetFilteredAsync(
            query.Email,
            query.IsActive,
            query.CreatedFrom,
            query.CreatedTo,
            query.SortBy,
            query.SortDirection,
            query.Page,
            query.PageSize,
            cancellationToken);

        PagedResult<UserResponse> response = new(
            paged.Items.Select(u => new UserResponse(
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email!,
                u.IsActive,
                u.CreatedAtUtc)).ToList(),
            paged.TotalCount,
            paged.Page,
            paged.PageSize);

        return Result.Success(response);
    }
}