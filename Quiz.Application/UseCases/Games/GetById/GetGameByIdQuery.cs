using Quiz.Application.Abstractions.Messaging;

namespace Quiz.Application.UseCases.Games.GetById;

public sealed record GetGameByIdQuery(Guid GameId) : IQuery<GetGameByIdResponse>;
