using MediatR;
using Quiz.Api.Abstractions.Endpoints;
using Quiz.Api.Extensions.ApiResults;
using Quiz.Application.UseCases.Quotes.Delete;
using Quiz.Common.Constants;
using Quiz.Common.Results;
using Quiz.Domain.Roles;

namespace Quiz.Api.Endpoints.Quotes;

internal sealed class DeleteQuoteEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/quotes/{id:guid}", async (Guid id, ISender sender) =>
        {
            Result result = await sender.Send(new DeleteQuoteCommand(id));

            return result.IsSuccess ? Results.NoContent() : ApiResult.Problem(result);
        })
        .WithTags(Tags.Quotes)
        .WithName("DeleteQuote")
        .RequireAuthorization(p => p.RequireRole(Role.Names.Admin));
    }
}