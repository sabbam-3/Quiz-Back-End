using Quiz.Common.Results;

namespace Quiz.Domain.Quotes;

public static class QuoteErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("Quotes.NotFound", $"The quote with the ID '{id}' was not found");

    public static Error AlreadyDeleted(Guid id) =>
        Error.Failure("Quotes.AlreadyDeleted", $"The quote with the ID '{id}' has already been deleted");

    public static Error DuplicateQuote(string authorName, string content) =>
        Error.Conflict("Quotes.DuplicateQuote", $"A quote by '{authorName}' with the same content already exists");

    public static Error InsufficientQuotesForMultipleChoice() =>
        Error.Problem("Quotes.InsufficientQuotesForMultipleChoice", "Not enough quotes are available to generate multiple choice options (minimum 3 required)");
}
