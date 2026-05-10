using Microsoft.EntityFrameworkCore;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Domain.Roles;
using Quiz.Infrastructure.Database;

namespace Quiz.Infrastructure.Repositories;

internal sealed class UserRoleRepository(ApplicationDbContext context) : IUserRoleRepository
{
    public async Task<string?> GetRoleNameByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleName)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task AddAsync(UserRole userRole, CancellationToken cancellationToken = default)
    {
        await context.UserRoles.AddAsync(userRole, cancellationToken);
    }
}