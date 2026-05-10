using Quiz.Application.Abstractions.Messaging;

namespace Quiz.Application.UseCases.Auth.Register;

public sealed record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : ICommand<Guid>;