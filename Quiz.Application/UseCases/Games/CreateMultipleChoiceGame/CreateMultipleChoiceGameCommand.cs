using Quiz.Application.Abstractions.Messaging;

namespace Quiz.Application.UseCases.Games.CreateMultipleChoiceGame;

public sealed record CreateMultipleChoiceGameCommand : ICommand<Guid>;
