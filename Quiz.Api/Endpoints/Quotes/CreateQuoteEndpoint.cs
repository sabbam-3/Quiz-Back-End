using MediatR;
using Quiz.Api.Abstractions.Endpoints;
using Quiz.Api.Extensions.ApiResults;
using Quiz.Application.UseCases.Quotes.Create;
using Quiz.Common.Constants;
using Quiz.Common.Results;
using Quiz.Domain.Roles;

namespace Quiz.Api.Endpoints.Quotes;

internal sealed class CreateQuoteEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/quotes", async (CreateQuoteCommand command, ISender sender) =>
        {
            Result<Guid> result = await sender.Send(command);

            return result.IsSuccess
                ? Results.CreatedAtRoute("GetQuoteById", new { id = result.Value }, result.Value)
                : ApiResult.Problem(result);
        })
        .WithTags(Tags.Quotes)
        .WithName("CreateQuote")
        .RequireAuthorization(p => p.RequireRole(Role.Names.Admin));
    }
}
