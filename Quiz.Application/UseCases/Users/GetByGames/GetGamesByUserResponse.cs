using Quiz.Domain.Games;

namespace Quiz.Application.UseCases.Users.GetByGames;

public sealed record GetGamesByUserResponse(
    Guid Id,
    Guid UserId,
    QuizMode Mode,
    GameStatus Status,
    float Score,
    DateTime CreatedAtUtc);