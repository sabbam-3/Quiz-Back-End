using MediatR;
using Microsoft.AspNetCore.Mvc;
using Quiz.Api.Abstractions.Endpoints;
using Quiz.Api.Extensions.ApiResults;
using Quiz.Application.UseCases.Games.AnswerMultipleChoiceQuestion;
using Quiz.Application.UseCases.Games.AnswerQuestion;
using Quiz.Common.Constants;
using Quiz.Common.Results;
using Quiz.Domain.Roles;

namespace Quiz.Api.Endpoints.Games;

internal sealed class AnswerMultipleChoiceQuestionEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/games/{id:guid}/multiplechoice/questions/{questionId:guid}/answer", async ([FromRoute] Guid id, [FromRoute] Guid questionId, [FromBody] AnswerMultipleChoiceQuestionRequest request, ISender sender) =>
        {
            Result<AnswerQuestionResponse> result = await sender.Send(new AnswerMultipleChoiceQuestionCommand(id, questionId, request.Answer));

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : ApiResult.Problem(result);
        })
        .WithTags(Tags.Games)
        .WithName("AnswerMultipleChoiceQuestion")
        .RequireAuthorization(p => p.RequireRole(Role.Names.User, Role.Names.Admin));
    }
}

internal sealed record AnswerMultipleChoiceQuestionRequest(string Answer);