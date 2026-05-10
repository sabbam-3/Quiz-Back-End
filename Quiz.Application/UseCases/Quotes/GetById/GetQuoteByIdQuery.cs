using Quiz.Application.Abstractions.Messaging;

namespace Quiz.Application.UseCases.Quotes.GetById;

public sealed record GetQuoteByIdQuery(Guid QuoteId) : IQuery<QuoteResponse>;
