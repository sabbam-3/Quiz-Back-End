using Microsoft.EntityFrameworkCore;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Common.Results;
using Quiz.Domain.Games;
using Quiz.Infrastructure.Database;

namespace Quiz.Infrastructure.Repositories;

internal sealed class GameRepository(ApplicationDbContext context) : IGameRepository
{
    public async Task<Game?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Games
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
    }

    public async Task<Game?> GetByIdWithQuestionsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Games
            .Include(g => g.Questions)
                .ThenInclude(gq => gq.Quote)
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
    }

    public async Task<Game?> GetByIdWithUnansweredQuestionsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Games
            .AsNoTracking()
            .Include(g => g.Questions.Where(q => q.AnswerGiven == null))
                .ThenInclude(gq => gq.Quote)
            .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Game>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await context.Games
            .AsNoTracking()
            .Include(g => g.Questions)
                .ThenInclude(gq => gq.Quote)
            .Where(g => g.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedResult<Game>> GetFilteredAsync(
        Guid? userId,
        QuizMode? mode,
        GameStatus? status,
        DateTime? createdFrom,
        DateTime? createdTo,
        string sortBy,
        string sortDirection,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Game> query = context.Games
            .AsNoTracking()
            .Include(g => g.User);

        if (userId is not null)
            query = query.Where(g => g.UserId == userId);

        if (mode is not null)
            query = query.Where(g => g.Mode == mode);

        if (status is not null)
            query = query.Where(g => g.Status == status);

        if (createdFrom is not null)
            query = query.Where(g => g.CreatedAtUtc >= createdFrom);

        if (createdTo is not null)
            query = query.Where(g => g.CreatedAtUtc <= createdTo);

        bool descending = sortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);

        query = sortBy.ToLowerInvariant() switch
        {
            "status" => descending ? query.OrderByDescending(g => g.Status) : query.OrderBy(g => g.Status),
            "mode"   => descending ? query.OrderByDescending(g => g.Mode)   : query.OrderBy(g => g.Mode),
            _        => descending ? query.OrderByDescending(g => g.CreatedAtUtc) : query.OrderBy(g => g.CreatedAtUtc),
        };

        int totalCount = await query.CountAsync(cancellationToken);

        List<Game> items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Game>(items, totalCount, page, pageSize);
    }

    public async Task AddAsync(Game game, CancellationToken cancellationToken = default)
    {
        await context.Games.AddAsync(game, cancellationToken);
    }
}