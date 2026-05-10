using MediatR;
using Microsoft.AspNetCore.Mvc;
using Quiz.Api.Abstractions.Endpoints;
using Quiz.Api.Extensions.ApiResults;
using Quiz.Application.UseCases.Games.AnswerBinaryQuestion;
using Quiz.Application.UseCases.Games.AnswerQuestion;
using Quiz.Common.Constants;
using Quiz.Common.Results;
using Quiz.Domain.Roles;

namespace Quiz.Api.Endpoints.Games;

internal sealed class AnswerBinaryQuestionEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/games/{id:guid}/binary/questions/{questionId:guid}/answer", async ([FromRoute] Guid id, [FromRoute] Guid questionId, [FromBody] AnswerBinaryQuestionRequest request, ISender sender) =>
        {
            Result<AnswerQuestionResponse> result = await sender.Send(new AnswerBinaryQuestionCommand(id, questionId, request.Answer));

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : ApiResult.Problem(result);
        })
        .WithTags(Tags.Games)
        .WithName("AnswerBinaryQuestion")
        .RequireAuthorization(p => p.RequireRole(Role.Names.User, Role.Names.Admin));
    }
}

internal sealed record AnswerBinaryQuestionRequest(string Answer);