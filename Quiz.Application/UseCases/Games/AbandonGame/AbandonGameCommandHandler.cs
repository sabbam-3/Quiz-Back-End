using Quiz.Application.Abstractions.Authentication;
using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Common.Results;
using Quiz.Domain.Games;

namespace Quiz.Application.UseCases.Games.AbandonGame;

internal sealed class AbandonGameCommandHandler(IGameRepository gameRepository, IUserContext userContext, IUnitOfWork unitOfWork) : ICommandHandler<AbandonGameCommand>
{
    public async Task<Result> Handle(AbandonGameCommand request, CancellationToken cancellationToken)
    {
        Game? game = await gameRepository.GetByIdWithQuestionsAsync(request.GameId, cancellationToken);
        
        if (game is null)
        {
            return Result.Failure(GameErrors.NotFound(request.GameId));
        }

        if(game.IsCompleted)
        {
            return Result.Failure(GameErrors.AlreadyCompleted(request.GameId));
        }

        var userId = userContext.UserId;

        if (game.UserId != userId)
        {
            return Result.Failure(GameErrors.UserNotFound(userId));
        }

        game.AbandonAllUnansweredQuestions();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}