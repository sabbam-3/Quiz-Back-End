using Quiz.Application.Abstractions.Authentication;
using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Common.Results;
using Quiz.Domain.Games;
using Quiz.Domain.Quotes;
using Quiz.Domain.Users;

namespace Quiz.Application.UseCases.Games.CreateMultipleChoiceGame;

internal sealed class CreateMultipleChoiceGameCommandHandler(
    IGameRepository gameRepository,
    IUserRepository userRepository,
    IQuoteRepository quoteRepository,
    IUserContext context,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateMultipleChoiceGameCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateMultipleChoiceGameCommand command, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(context.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure<Guid>(GameErrors.UserNotFound(context.UserId));
        }

        IReadOnlyCollection<Quote> randomQuotes = await quoteRepository.GetRandomQuotesAsync(cancellationToken: cancellationToken);

        Result<Game> game = Game.CreateMultipleChoice(context.UserId, randomQuotes);

        if (game.IsFailure)
        {
            return Result.Failure<Guid>(game.Error);
        }

        await gameRepository.AddAsync(game.Value, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(game.Value.Id);
    }
}