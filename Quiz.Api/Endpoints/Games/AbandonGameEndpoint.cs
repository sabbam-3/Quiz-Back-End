using MediatR;
using Quiz.Api.Abstractions.Endpoints;
using Quiz.Api.Extensions.ApiResults;
using Quiz.Application.UseCases.Games.AbandonGame;
using Quiz.Common.Constants;
using Quiz.Domain.Roles;

namespace Quiz.Api.Endpoints.Games;

public sealed class AbandonGameEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("games/{id:guid}/abandon", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new AbandonGameCommand(id));

            return result.IsSuccess ? Results.Ok(result) : ApiResult.Problem(result);
        })
        .WithTags(Tags.Games)
        .RequireAuthorization(p => p.RequireRole(Role.Names.User, Role.Names.Admin));
    }
}
