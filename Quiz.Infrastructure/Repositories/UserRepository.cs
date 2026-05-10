using Microsoft.EntityFrameworkCore;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Common.Results;
using Quiz.Domain.Users;
using Quiz.Infrastructure.Database;

namespace Quiz.Infrastructure.Repositories;

internal sealed class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<PagedResult<User>> GetFilteredAsync(
        string? email,
        bool? isActive,
        DateTime? createdFrom,
        DateTime? createdTo,
        string sortBy,
        string sortDirection,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        IQueryable<User> query = context.Users.AsNoTracking();

        if (email is not null)
            query = query.Where(u => u.Email!.Contains(email));

        if (isActive is not null)
            query = query.Where(u => u.IsActive == isActive);

        if (createdFrom is not null)
            query = query.Where(u => u.CreatedAtUtc >= createdFrom);

        if (createdTo is not null)
            query = query.Where(u => u.CreatedAtUtc <= createdTo);

        bool descending = sortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);

        query = sortBy.ToLowerInvariant() switch
        {
            "email"     => descending ? query.OrderByDescending(u => u.Email)       : query.OrderBy(u => u.Email),
            "firstname" => descending ? query.OrderByDescending(u => u.FirstName)   : query.OrderBy(u => u.FirstName),
            "lastname"  => descending ? query.OrderByDescending(u => u.LastName)    : query.OrderBy(u => u.LastName),
            _           => descending ? query.OrderByDescending(u => u.CreatedAtUtc): query.OrderBy(u => u.CreatedAtUtc),
        };

        int totalCount = await query.CountAsync(cancellationToken);

        List<User> items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<User>(items, totalCount, page, pageSize);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await context.Users.AddAsync(user, cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<User?> GetByRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .Join(
                context.UserTokens.Where(t =>
                    t.LoginProvider == "Quiz" &&
                    t.Name == "RefreshToken" &&
                    t.Value == refreshToken),
                user => user.IdentityId,
                token => token.UserId,
                (user, _) => user)
            .FirstOrDefaultAsync(cancellationToken);
    }
}