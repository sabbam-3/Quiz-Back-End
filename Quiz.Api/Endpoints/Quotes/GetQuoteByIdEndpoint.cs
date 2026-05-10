using MediatR;
using Quiz.Api.Abstractions.Endpoints;
using Quiz.Api.Extensions.ApiResults;
using Quiz.Application.UseCases.Quotes.GetById;
using Quiz.Common.Constants;
using Quiz.Common.Results;
using Quiz.Domain.Roles;

namespace Quiz.Api.Endpoints.Quotes;

internal sealed class GetQuoteByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/quotes/{id:guid}", async (Guid id, ISender sender) =>
        {
            Result<QuoteResponse> result = await sender.Send(new GetQuoteByIdQuery(id));

            return result.IsSuccess ? Results.Ok(result.Value) : ApiResult.Problem(result);
        })
        .WithTags(Tags.Quotes)
        .WithName("GetQuoteById")
        .RequireAuthorization(p => p.RequireRole(Role.Names.User, Role.Names.Admin));
    }
}
