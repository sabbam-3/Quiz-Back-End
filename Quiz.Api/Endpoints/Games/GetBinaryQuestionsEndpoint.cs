using MediatR;
using Quiz.Api.Abstractions.Endpoints;
using Quiz.Api.Extensions.ApiResults;
using Quiz.Application.UseCases.Games.GetBinaryQuestions;
using Quiz.Common.Constants;
using Quiz.Common.Results;
using Quiz.Domain.Roles;

namespace Quiz.Api.Endpoints.Games;

internal sealed class GetBinaryQuestionsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/games/{id:guid}/binary/questions", async (Guid id, ISender sender) =>
        {
            Result<GetBinaryQuestionsResponse> result = await sender.Send(new GetBinaryQuestionsQuery(id));

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : ApiResult.Problem(result);
        })
        .WithTags(Tags.Games)
        .WithName("GetBinaryQuestions")
        .RequireAuthorization(p => p.RequireRole(Role.Names.User, Role.Names.Admin));
    }
}