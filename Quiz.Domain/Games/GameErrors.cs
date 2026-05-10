using Quiz.Common.Results;

namespace Quiz.Domain.Games;

public static class GameErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("Games.NotFound", $"The game with the ID '{id}' was not found");

    public static Error IncorrectQuizMode(Guid id) => Error.Failure("Games.IncorrectQuizMode", $"The game with the ID '{id}' has different quiz mode");

    public static Error AlreadyDeleted(Guid id) =>
        Error.Failure("Games.AlreadyDeleted", $"The game with the ID '{id}' has already been deleted");

    public static Error AlreadyCompleted(Guid id) =>
        Error.Failure("Games.AlreadyCompleted", $"The game with the ID '{id}' has already been completed and cannot be modified");

    public static Error NoQuotesAvailable() =>
        Error.Problem("Games.NoQuotesAvailable", "There are no active quotes available to start a new game");

    public static Error NotEnoughAuthors() =>
        Error.Problem("Games.NotEnoughAuthors", "There are not enough active authors available to start a new game");

    public static Error UserNotFound(Guid userId) =>
        Error.NotFound("Games.UserNotFound", $"Cannot start a game for the user with ID '{userId}' because the user was not found");

    public static Error Unauthorized(Guid id) =>
        Error.Failure("Games.Unauthorized", $"You do not have access to the game with the ID '{id}'");

    public static Error NoCurrentQuestion(Guid id) =>
        Error.NotFound("Games.NoCurrentQuestion", $"The game with the ID '{id}' has no unanswered question");
}