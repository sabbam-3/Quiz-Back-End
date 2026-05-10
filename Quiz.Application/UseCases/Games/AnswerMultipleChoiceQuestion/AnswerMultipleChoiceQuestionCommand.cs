using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.UseCases.Games.AnswerQuestion;

namespace Quiz.Application.UseCases.Games.AnswerMultipleChoiceQuestion;

public sealed record AnswerMultipleChoiceQuestionCommand(Guid GameId, Guid QuestionId, string Answer) : ICommand<AnswerQuestionResponse>;