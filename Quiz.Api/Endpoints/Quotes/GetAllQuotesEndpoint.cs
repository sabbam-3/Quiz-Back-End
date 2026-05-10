using MediatR;
using Quiz.Api.Abstractions.Endpoints;
using Quiz.Api.Extensions.ApiResults;
using Quiz.Application.UseCases.Quotes.GetAll;
using Quiz.Application.UseCases.Quotes.GetById;
using Quiz.Common.Constants;
using Quiz.Common.Results;
using Quiz.Domain.Roles;

namespace Quiz.Api.Endpoints.Quotes;

internal sealed class GetAllQuotesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/quotes", async (
            string? authorName,
            bool? isActive,
            DateTime? createdFrom,
            DateTime? createdTo,
            string? sortBy,
            string? sortDirection,
            ISender sender,
            int page = 1,
            int pageSize = 10) =>
        {
            Result<PagedResult<QuoteResponse>> result = await sender.Send(new GetAllQuotesQuery(
                authorName,
                isActive,
                createdFrom,
                createdTo,
                sortBy ?? "createdAt",
                sortDirection ?? "asc",
                page,
                pageSize));

            return result.IsSuccess ? Results.Ok(result.Value) : ApiResult.Problem(result);
        })
        .WithTags(Tags.Quotes)
        .WithName("GetAllQuotes")
        .RequireAuthorization(p => p.RequireRole(Role.Names.Admin));
    }
}