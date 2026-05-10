using Quiz.Common.Results;

namespace Quiz.Domain.Games;

public static class GameQuestionErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("GameQuestions.NotFound", $"The game question with the ID '{id}' was not found");

    public static Error AlreadyAnswered(Guid id) =>
        Error.Failure("GameQuestions.AlreadyAnswered", $"The question with the ID '{id}' has already been answered");

    public static Error NotPartOfGame(Guid questionId, Guid gameId) =>
        Error.Failure("GameQuestions.NotPartOfGame", $"The question with the ID '{questionId}' does not belong to the game with the ID '{gameId}'");

    public static Error InvalidBinaryAnswer(string answer) =>
        Error.Failure("GameQuestions.InvalidBinaryAnswer", $"'{answer}' is not a valid binary answer. Expected 'Yes' or 'No'");

    public static Error InvalidChoiceAnswer(string answer) =>
        Error.Failure("GameQuestions.InvalidChoiceAnswer", $"'{answer}' is not one of the available choices for this question");
}