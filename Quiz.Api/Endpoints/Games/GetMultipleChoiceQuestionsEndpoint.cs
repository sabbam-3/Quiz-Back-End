using MediatR;
using Quiz.Api.Abstractions.Endpoints;
using Quiz.Api.Extensions.ApiResults;
using Quiz.Application.UseCases.Games.GetMultipleChoiceQuestions;
using Quiz.Common.Constants;
using Quiz.Common.Results;
using Quiz.Domain.Roles;

namespace Quiz.Api.Endpoints.Games;

public class GetMultipleChoiceQuestionsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/games/{id:guid}/multiplechoice/questions", async (Guid id, ISender sender) =>
        {
            Result<GetMultipleChoiceQuestionsResponse> result = await sender.Send(new GetMultipleChoiceQuestionsQuery(id));

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : ApiResult.Problem(result);
        })
        .WithTags(Tags.Games)
        .WithName("GetMultipleChoiceQuestions")
        .RequireAuthorization(p => p.RequireRole(Role.Names.User, Role.Names.Admin));
    }
}