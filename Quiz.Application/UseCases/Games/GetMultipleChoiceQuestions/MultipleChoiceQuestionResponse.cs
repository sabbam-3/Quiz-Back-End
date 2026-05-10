namespace Quiz.Application.UseCases.Games.GetMultipleChoiceQuestions;

public sealed record MultipleChoiceQuestionResponse(
    Guid QuestionId,
    string QuoteContent,
    bool IsAnswered,
    IReadOnlyList<string> Options);