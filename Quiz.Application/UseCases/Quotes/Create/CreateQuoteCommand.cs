using Quiz.Application.Abstractions.Messaging;

namespace Quiz.Application.UseCases.Quotes.Create;

public sealed record CreateQuoteCommand(
    string Content,
    string AuthorName) : ICommand<Guid>;
