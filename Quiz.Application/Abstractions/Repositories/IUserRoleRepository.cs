using Quiz.Domain.Roles;

namespace Quiz.Application.Abstractions.Repositories;

public interface IUserRoleRepository
{
    Task<string?> GetRoleNameByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task AddAsync(UserRole userRole, CancellationToken cancellationToken = default);
}