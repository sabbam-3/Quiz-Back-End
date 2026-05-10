using Quiz.Application.Abstractions.Messaging;

namespace Quiz.Application.UseCases.Auth.Login;

public sealed record LoginCommand(string Email, string Password) : ICommand<LoginResponse>;