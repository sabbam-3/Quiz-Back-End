using Quiz.Common.Results;
using Quiz.Domain.Quotes;

namespace Quiz.Application.Abstractions.Repositories;

public interface IQuoteRepository
{
    Task<Quote?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<PagedResult<Quote>> GetFilteredAsync(
        string? authorName,
        bool? isActive,
        DateTime? createdFrom,
        DateTime? createdTo,
        string sortBy,
        string sortDirection,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Quote>> GetActiveQuotesAsync(int batchSize = 10, CancellationToken cancellationToken = default);
    
    Task<IReadOnlyCollection<Quote>> GetRandomQuotesAsync(int batchSize = 10, CancellationToken cancellationToken = default);
    
    Task AddAsync(Quote quote, CancellationToken cancellationToken = default);

    Task<bool> ExistsByAuthorNameAndContentAsync(string authorName, string content, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<string>> GetAllDistinctAuthorNamesAsync(int batchSize = 10, CancellationToken cancellationToken = default);
}