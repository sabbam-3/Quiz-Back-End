using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.UseCases.Games.AnswerQuestion;

namespace Quiz.Application.UseCases.Games.AnswerBinaryQuestion;

public sealed record AnswerBinaryQuestionCommand(Guid GameId, Guid QuestionId, string Answer) : ICommand<AnswerQuestionResponse>;
