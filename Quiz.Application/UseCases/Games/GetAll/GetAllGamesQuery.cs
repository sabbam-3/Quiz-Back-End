using Quiz.Application.Abstractions.Messaging;
using Quiz.Common.Results;
using Quiz.Domain.Games;

namespace Quiz.Application.UseCases.Games.GetAll;

public sealed record GetAllGamesQuery(
    Guid? UserId = null,
    QuizMode? Mode = null,
    GameStatus? Status = null,
    DateTime? CreatedFrom = null,
    DateTime? CreatedTo = null,
    string SortBy = "createdAt",
    string SortDirection = "asc",
    int Page = 1,
    int PageSize = 10) : IQuery<PagedResult<GetAllGamesResponse>>;