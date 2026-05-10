using Microsoft.EntityFrameworkCore;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Common.Results;
using Quiz.Domain.Quotes;
using Quiz.Infrastructure.Database;

namespace Quiz.Infrastructure.Repositories;

internal sealed class QuoteRepository(ApplicationDbContext context) : IQuoteRepository
{
    public async Task<Quote?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Quotes
            .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Quote>> GetActiveQuotesAsync(int batchSize = 10,  CancellationToken cancellationToken = default)
    {
        return await context.Quotes
            .AsNoTracking()
            .Where(q => q.IsActive)
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<PagedResult<Quote>> GetFilteredAsync(
        string? authorName,
        bool? isActive,
        DateTime? createdFrom,
        DateTime? createdTo,
        string sortBy,
        string sortDirection,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Quote> query = context.Quotes.AsNoTracking();

        if (authorName is not null)
            query = query.Where(q => q.AuthorName.Contains(authorName));

        if (isActive is not null)
            query = query.Where(q => q.IsActive == isActive);

        if (createdFrom is not null)
            query = query.Where(q => q.CreatedAtUtc >= createdFrom);

        if (createdTo is not null)
            query = query.Where(q => q.CreatedAtUtc <= createdTo);

        bool descending = sortDirection.Equals("desc", StringComparison.OrdinalIgnoreCase);

        query = sortBy.ToLowerInvariant() switch
        {
            "authorname" => descending ? query.OrderByDescending(q => q.AuthorName)   : query.OrderBy(q => q.AuthorName),
            "content"    => descending ? query.OrderByDescending(q => q.Content)      : query.OrderBy(q => q.Content),
            _            => descending ? query.OrderByDescending(q => q.CreatedAtUtc) : query.OrderBy(q => q.CreatedAtUtc),
        };

        int totalCount = await query.CountAsync(cancellationToken);

        List<Quote> items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Quote>(items, totalCount, page, pageSize);
    }

    public async Task AddAsync(Quote quote, CancellationToken cancellationToken = default)
    {
        await context.Quotes.AddAsync(quote, cancellationToken);
    }

    public async Task<bool> ExistsByAuthorNameAndContentAsync(string authorName, string content, CancellationToken cancellationToken = default)
    {
        return await context.Quotes
            .AnyAsync(q => q.AuthorName == authorName && q.Content == content, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Quote>> GetRandomQuotesAsync(int batchSize = 10, CancellationToken cancellationToken = default)
    {
        return await context.Quotes
            .AsNoTracking()
            .Where(q => q.IsActive)
            .GroupBy(q => q.AuthorName)
            .Select(g => g.OrderBy(_ => Guid.NewGuid()).First())
            .OrderBy(_ => Guid.NewGuid())
            .Take(batchSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<string>> GetAllDistinctAuthorNamesAsync(int batchSize = 10, CancellationToken cancellationToken = default)
    {
        return await context.Quotes
            .AsNoTracking()
            .Where(q => q.IsActive)
            .Select(q => q.AuthorName)
            .Distinct()
            .Take(10)
            .ToListAsync(cancellationToken);
    }
}