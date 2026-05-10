using MediatR;
using Quiz.Api.Abstractions.Endpoints;
using Quiz.Api.Extensions.ApiResults;
using Quiz.Application.UseCases.Games.CreateMultipleChoiceGame;
using Quiz.Common.Constants;
using Quiz.Common.Results;
using Quiz.Domain.Roles;

namespace Quiz.Api.Endpoints.Games;

internal sealed class CreateMultipleChoiceGameEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/games/multiplechoice", async (ISender sender) =>
        {
            Result<Guid> result = await sender.Send(new CreateMultipleChoiceGameCommand());

            return result.IsSuccess
                ? Results.CreatedAtRoute("GetGameById", new { id = result.Value }, result.Value)
                : ApiResult.Problem(result);
        })
        .WithTags(Tags.Games)
        .WithName("CreateMultipleChoiceGame")
        .RequireAuthorization(p => p.RequireRole(Role.Names.User, Role.Names.Admin));
    }
}
