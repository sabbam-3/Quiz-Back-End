using Quiz.Application.Abstractions.Messaging;

namespace Quiz.Application.UseCases.Users.Update;

public sealed record UpdateUserCommand(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email) : ICommand;
