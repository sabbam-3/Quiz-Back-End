using Quiz.Application.Abstractions.Messaging;

namespace Quiz.Application.UseCases.Games.AbandonGame;

public sealed record AbandonGameCommand(Guid GameId) : ICommand;