using Quiz.Application.Abstractions.Messaging;

namespace Quiz.Application.UseCases.Quotes.Delete;

public sealed record DeleteQuoteCommand(Guid QuoteId) : ICommand;
