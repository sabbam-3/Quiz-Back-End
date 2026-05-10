using Quiz.Application.Abstractions.Messaging;

namespace Quiz.Application.UseCases.Games.GetBinaryQuestions;

public sealed record GetBinaryQuestionsQuery(Guid GameId) : IQuery<GetBinaryQuestionsResponse>;