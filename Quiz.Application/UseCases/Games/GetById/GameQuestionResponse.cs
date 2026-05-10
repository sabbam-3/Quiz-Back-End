namespace Quiz.Application.UseCases.Games.GetById;

public sealed record GameQuestionResponse(
    Guid Id,
    Guid QuoteId,
    string QuoteContent,
    string CorrectAuthor,
    string? AnswerGiven,
    string? SuggestedAuthorName,
    bool? IsCorrect,
    DateTime CreatedAtUtc,
    DateTime? AnsweredAtUtc);
