using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Application.UseCases.Quotes.GetById;
using Quiz.Common.Results;
using Quiz.Domain.Quotes;

namespace Quiz.Application.UseCases.Quotes.GetAll;

internal sealed class GetAllQuotesQueryHandler(
    IQuoteRepository quoteRepository) : IQueryHandler<GetAllQuotesQuery, PagedResult<QuoteResponse>>
{
    public async Task<Result<PagedResult<QuoteResponse>>> Handle(GetAllQuotesQuery query, CancellationToken cancellationToken)
    {
        PagedResult<Quote> paged = await quoteRepository.GetFilteredAsync(
            query.AuthorName,
            query.IsActive,
            query.CreatedFrom,
            query.CreatedTo,
            query.SortBy,
            query.SortDirection,
            query.Page,
            query.PageSize,
            cancellationToken);

        PagedResult<QuoteResponse> response = new(
            paged.Items.Select(q => new QuoteResponse(
                q.Id,
                q.Content,
                q.AuthorName,
                q.IsActive,
                q.CreatedAtUtc)).ToList(),
            paged.TotalCount,
            paged.Page,
            paged.PageSize);

        return Result.Success(response);
    }
}
