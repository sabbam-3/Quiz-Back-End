using Quiz.Domain.Games;

namespace Quiz.Application.UseCases.Games.GetById;

public sealed record GetGameByIdResponse(
    Guid Id,
    Guid UserId,
    QuizMode Mode,
    GameStatus Status,
    float Score,
    DateTime CreatedAtUtc,
    IReadOnlyCollection<GameQuestionResponse> Questions);