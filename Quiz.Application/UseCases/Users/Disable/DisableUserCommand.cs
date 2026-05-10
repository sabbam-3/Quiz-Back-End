using Quiz.Application.Abstractions.Messaging;

namespace Quiz.Application.UseCases.Users.Disable;

public sealed record DisableUserCommand(Guid UserId) : ICommand;
