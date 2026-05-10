using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.UseCases.Auth.Login;

namespace Quiz.Application.UseCases.Auth.RefreshToken;

public sealed record RefreshTokenCommand(string RefreshToken) : ICommand<LoginResponse>;
