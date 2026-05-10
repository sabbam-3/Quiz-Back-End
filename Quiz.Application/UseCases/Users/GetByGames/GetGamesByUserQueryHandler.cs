using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Common.Results;
using Quiz.Domain.Games;
using Quiz.Domain.Users;

namespace Quiz.Application.UseCases.Users.GetByGames;

internal sealed class GetGamesByUserQueryHandler(
    IGameRepository gameRepository,
    IUserRepository userRepository) : IQueryHandler<GetGamesByUserQuery, IReadOnlyCollection<GetGamesByUserResponse>>
{
    public async Task<Result<IReadOnlyCollection<GetGamesByUserResponse>>> Handle(GetGamesByUserQuery query, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(query.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure<IReadOnlyCollection<GetGamesByUserResponse>>(UserErrors.NotFound(query.UserId));
        }

        IReadOnlyCollection<Game> games = await gameRepository.GetByUserIdAsync(query.UserId, cancellationToken);

        IReadOnlyCollection<GetGamesByUserResponse> response = games
            .Select(MapToResponse)
            .ToList();

        return Result.Success(response);
    }

    private static GetGamesByUserResponse MapToResponse(Game game) =>
        new(
            game.Id,
            game.UserId,
            game.Mode,
            game.Status,
            game.Score,
            game.CreatedAtUtc);
}
