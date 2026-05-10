namespace Quiz.Application.UseCases.Games.GetBinaryQuestions;

public sealed record GetBinaryQuestionsResponse(Guid GameId, IReadOnlyCollection<BinaryQuestionResponse> Questions);