namespace Quiz.Application.UseCases.Games.AnswerQuestion;

public sealed record AnswerQuestionResponse(
    bool IsCorrect,
    string CorrectAuthorName,
    bool IsGameCompleted);