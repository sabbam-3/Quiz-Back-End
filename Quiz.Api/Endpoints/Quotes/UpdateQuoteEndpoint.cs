using MediatR;
using Quiz.Api.Abstractions.Endpoints;
using Quiz.Api.Extensions.ApiResults;
using Quiz.Application.UseCases.Quotes.Update;
using Quiz.Common.Constants;
using Quiz.Common.Results;
using Quiz.Domain.Roles;

namespace Quiz.Api.Endpoints.Quotes;

internal sealed class UpdateQuoteEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/quotes/{id:guid}", async (Guid id, UpdateQuoteRequest request, ISender sender) =>
        {
            UpdateQuoteCommand command = new(id, request.Content, request.AuthorName);

            Result result = await sender.Send(command);

            return result.IsSuccess ? Results.NoContent() : ApiResult.Problem(result);
        })
        .WithTags(Tags.Quotes)
        .WithName("UpdateQuote")
        .RequireAuthorization(p => p.RequireRole(Role.Names.Admin));
    }

    internal sealed record UpdateQuoteRequest(string Content, string AuthorName);
}
