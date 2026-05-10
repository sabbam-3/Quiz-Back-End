using Quiz.Application.Abstractions.Authentication;
using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Application.UseCases.Games.AnswerQuestion;
using Quiz.Common.Results;
using Quiz.Domain.Games;

namespace Quiz.Application.UseCases.Games.AnswerMultipleChoiceQuestion;

internal sealed class AnswerMultipleChoiceQuestionCommandHandler(
    IGameRepository gameRepository,
    IUserContext userContext,
    IUnitOfWork unitOfWork) : ICommandHandler<AnswerMultipleChoiceQuestionCommand, AnswerQuestionResponse>
{
    public async Task<Result<AnswerQuestionResponse>> Handle(
        AnswerMultipleChoiceQuestionCommand command,
        CancellationToken cancellationToken)
    {
        Game? game = await gameRepository.GetByIdWithQuestionsAsync(command.GameId, cancellationToken);
        if (game is null)
        {
            return Result.Failure<AnswerQuestionResponse>(GameErrors.NotFound(command.GameId));
        }

        if (game.UserId != userContext.UserId)
        {
            return Result.Failure<AnswerQuestionResponse>(GameErrors.Unauthorized(command.GameId));
        }

        if (game.IsBinary)
        {
            return Result.Failure<AnswerQuestionResponse>(GameErrors.IncorrectQuizMode(command.GameId));
        }

        if (game.IsCompleted)
        {
            return Result.Failure<AnswerQuestionResponse>(GameErrors.AlreadyCompleted(command.GameId));
        }

        GameQuestion? current = game.Questions.FirstOrDefault(q => q.Id == command.QuestionId && !q.IsAnswered);
        if (current is null)
        {
            return Result.Failure<AnswerQuestionResponse>(GameErrors.NoCurrentQuestion(command.GameId));
        }

        current.AnswerMultipleChoice(command.Answer);

        if (game.IsAllQuestionsAnswered)
        {
            game.Complete();
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new AnswerQuestionResponse(current.IsCorrect!.Value, current.Quote.AuthorName, game.IsCompleted));
    }
}