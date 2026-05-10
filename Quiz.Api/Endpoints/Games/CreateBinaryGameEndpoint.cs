using MediatR;
using Quiz.Api.Abstractions.Endpoints;
using Quiz.Api.Extensions.ApiResults;
using Quiz.Application.UseCases.Games.CreateBinaryGame;
using Quiz.Common.Constants;
using Quiz.Common.Results;
using Quiz.Domain.Roles;

namespace Quiz.Api.Endpoints.Games;

internal sealed class CreateBinaryGameEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/games/binary", async (ISender sender) =>
        {
            Result<Guid> result = await sender.Send(new CreateBinaryGameCommand());

            return result.IsSuccess
                ? Results.CreatedAtRoute("GetGameById", new { id = result.Value }, result.Value)
                : ApiResult.Problem(result);
        })
        .WithTags(Tags.Games)
        .WithName("CreateBinaryGame")
        .RequireAuthorization(p => p.RequireRole(Role.Names.User, Role.Names.Admin));
    }
}