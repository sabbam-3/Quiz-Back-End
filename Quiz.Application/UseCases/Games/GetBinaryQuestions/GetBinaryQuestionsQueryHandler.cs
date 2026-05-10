using Quiz.Application.Abstractions.Authentication;
using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Common.Results;
using Quiz.Domain.Games;

namespace Quiz.Application.UseCases.Games.GetBinaryQuestions;

internal sealed class GetBinaryQuestionsQueryHandler(IUserContext userContext, IGameRepository gameRepository) : IQueryHandler<GetBinaryQuestionsQuery, GetBinaryQuestionsResponse>
{
    public async Task<Result<GetBinaryQuestionsResponse>> Handle(GetBinaryQuestionsQuery query, CancellationToken cancellationToken)
    {
        Game? game = await gameRepository.GetByIdWithUnansweredQuestionsAsync(query.GameId, cancellationToken);
        if (game is null)
        {
            return Result.Failure<GetBinaryQuestionsResponse>(GameErrors.NotFound(query.GameId));
        }

        if (game.UserId != userContext.UserId)
        {
            return Result.Failure<GetBinaryQuestionsResponse>(GameErrors.Unauthorized(query.GameId));
        }

        if (game.IsMultipleChoice)
        {
            return Result.Failure<GetBinaryQuestionsResponse>(GameErrors.IncorrectQuizMode(query.GameId));
        }

        if (game.IsCompleted)
        {
            return Result.Failure<GetBinaryQuestionsResponse>(GameErrors.AlreadyCompleted(query.GameId));
        }

        var binaryQuestions = new GetBinaryQuestionsResponse(
            game.Id,
            game.Questions
                .Select(q => new BinaryQuestionResponse(
                    q.Id,
                    q.Quote.Content,
                    q.SuggestedAuthorName!,
                    q.IsAnswered,
                    ["Yes", "No"])).ToList()
                    );

        return binaryQuestions;
    }
}