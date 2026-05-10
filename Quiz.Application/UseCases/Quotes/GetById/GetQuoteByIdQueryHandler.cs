using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Common.Results;
using Quiz.Domain.Quotes;

namespace Quiz.Application.UseCases.Quotes.GetById;

internal sealed class GetQuoteByIdQueryHandler(
    IQuoteRepository quoteRepository) : IQueryHandler<GetQuoteByIdQuery, QuoteResponse>
{
    public async Task<Result<QuoteResponse>> Handle(GetQuoteByIdQuery query, CancellationToken cancellationToken)
    {
        Quote? quote = await quoteRepository.GetByIdAsync(query.QuoteId, cancellationToken);
        if (quote is null)
        {
            return Result.Failure<QuoteResponse>(QuoteErrors.NotFound(query.QuoteId));
        }

        return Result.Success(new QuoteResponse(
            quote.Id,
            quote.Content,
            quote.AuthorName,
            quote.IsActive,
            quote.CreatedAtUtc));
    }
}
