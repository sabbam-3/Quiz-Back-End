using Quiz.Application.Abstractions.Messaging;

namespace Quiz.Application.UseCases.Quotes.Update;

public sealed record UpdateQuoteCommand(
    Guid QuoteId,
    string Content,
    string AuthorName) : ICommand;
