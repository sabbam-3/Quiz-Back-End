using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.UseCases.Quotes.GetById;
using Quiz.Common.Results;

namespace Quiz.Application.UseCases.Quotes.GetAll;

public sealed record GetAllQuotesQuery(
    string? AuthorName = null,
    bool? IsActive = null,
    DateTime? CreatedFrom = null,
    DateTime? CreatedTo = null,
    string SortBy = "createdAt",
    string SortDirection = "asc",
    int Page = 1,
    int PageSize = 10) : IQuery<PagedResult<QuoteResponse>>;
