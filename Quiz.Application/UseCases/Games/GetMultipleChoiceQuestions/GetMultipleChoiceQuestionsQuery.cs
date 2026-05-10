using Quiz.Application.Abstractions.Messaging;

namespace Quiz.Application.UseCases.Games.GetMultipleChoiceQuestions;

public sealed record GetMultipleChoiceQuestionsQuery(Guid GameId) : IQuery<GetMultipleChoiceQuestionsResponse>;