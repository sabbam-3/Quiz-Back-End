using Quiz.Common.Results;
using Quiz.Domain.Games;

namespace Quiz.Application.Abstractions.Repositories;

public interface IGameRepository
{
    Task<Game?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Game?> GetByIdWithQuestionsAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Game?> GetByIdWithUnansweredQuestionsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Game>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<PagedResult<Game>> GetFilteredAsync(
        Guid? userId,
        QuizMode? mode,
        GameStatus? status,
        DateTime? createdFrom,
        DateTime? createdTo,
        string sortBy,
        string sortDirection,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task AddAsync(Game game, CancellationToken cancellationToken = default);
}