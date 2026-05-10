using Quiz.Application.Abstractions.Messaging;

namespace Quiz.Application.UseCases.Users.Create;

public sealed record CreateUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string Role) : ICommand<Guid>;