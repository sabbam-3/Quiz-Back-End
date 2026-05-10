using Quiz.Domain.Games;

namespace Quiz.Application.UseCases.Games.GetAll;

public sealed record GetAllGamesResponse(
    Guid Id,
    Guid UserId,
    string UserEmail,
    QuizMode Mode,
    GameStatus Status,
    float Score,
    DateTime CreatedAtUtc);