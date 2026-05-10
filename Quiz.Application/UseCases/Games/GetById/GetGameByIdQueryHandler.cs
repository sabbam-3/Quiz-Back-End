using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Common.Results;
using Quiz.Domain.Games;

namespace Quiz.Application.UseCases.Games.GetById;

internal sealed class GetGameByIdQueryHandler(
    IGameRepository gameRepository) : IQueryHandler<GetGameByIdQuery, GetGameByIdResponse>
{
    public async Task<Result<GetGameByIdResponse>> Handle(GetGameByIdQuery query, CancellationToken cancellationToken)
    {
        Game? game = await gameRepository.GetByIdWithQuestionsAsync(query.GameId, cancellationToken);
        if (game is null)
        {
            return Result.Failure<GetGameByIdResponse>(GameErrors.NotFound(query.GameId));
        }

        return Result.Success(MapToResponse(game));
    }

    private static GetGameByIdResponse MapToResponse(Game game) =>
        new(
            game.Id,
            game.UserId,
            game.Mode,
            game.Status,
            game.Score,
            game.CreatedAtUtc,
            game.Questions
                .Select(q => new GameQuestionResponse(
                    q.Id,
                    q.QuoteId,
                    q.Quote.Content,
                    q.Quote.AuthorName,
                    q.AnswerGiven,
                    q.SuggestedAuthorName,
                    q.IsCorrect,
                    q.CreatedAtUtc,
                    q.AnsweredAtUtc))
                .ToList());
}
