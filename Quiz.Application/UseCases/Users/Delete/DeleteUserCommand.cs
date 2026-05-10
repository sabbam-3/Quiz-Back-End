using Quiz.Application.Abstractions.Messaging;

namespace Quiz.Application.UseCases.Users.Delete;

public sealed record DeleteUserCommand(Guid UserId) : ICommand;
