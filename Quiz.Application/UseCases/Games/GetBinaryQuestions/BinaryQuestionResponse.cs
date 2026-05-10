namespace Quiz.Application.UseCases.Games.GetBinaryQuestions;

public sealed record BinaryQuestionResponse(
    Guid QuestionId,
    string QuoteContent,
    string SuggestedAuthorName,
    bool IsAnswered,
    IReadOnlyList<string> Options);