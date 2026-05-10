using Quiz.Application.Abstractions.Messaging;

namespace Quiz.Application.UseCases.Auth.GetCurrentUser;

public sealed record GetCurrentUserQuery : IQuery<GetCurrentUserResponse>;