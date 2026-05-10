using Quiz.Application.Abstractions.Messaging;

namespace Quiz.Application.UseCases.Games.CreateBinaryGame;

public sealed record CreateBinaryGameCommand : ICommand<Guid>;
