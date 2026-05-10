using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Common.Results;
using Quiz.Domain.Games;

namespace Quiz.Application.UseCases.Games.GetAll;

internal sealed class GetAllGamesQueryHandler(
    IGameRepository gameRepository) : IQueryHandler<GetAllGamesQuery, PagedResult<GetAllGamesResponse>>
{
    public async Task<Result<PagedResult<GetAllGamesResponse>>> Handle(GetAllGamesQuery query, CancellationToken cancellationToken)
    {
        PagedResult<Game> paged = await gameRepository.GetFilteredAsync(
            query.UserId,
            query.Mode,
            query.Status,
            query.CreatedFrom,
            query.CreatedTo,
            query.SortBy,
            query.SortDirection,
            query.Page,
            query.PageSize,
            cancellationToken);

        PagedResult<GetAllGamesResponse> response = new(
            paged.Items.Select(MapToResponse).ToList(),
            paged.TotalCount,
            paged.Page,
            paged.PageSize);

        return Result.Success(response);
    }

    private static GetAllGamesResponse MapToResponse(Game game) =>
        new(game.Id, game.UserId, game.User.Email, game.Mode, game.Status, game.Score, game.CreatedAtUtc);
}