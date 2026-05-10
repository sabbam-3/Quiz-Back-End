using MediatR;
using Quiz.Api.Abstractions.Endpoints;
using Quiz.Api.Extensions.ApiResults;
using Quiz.Application.UseCases.Games.GetAll;
using Quiz.Common.Constants;
using Quiz.Common.Results;
using Quiz.Domain.Games;
using Quiz.Domain.Roles;

namespace Quiz.Api.Endpoints.Games;

internal sealed class GetAllGamesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/games", async (
            Guid? userId,
            QuizMode? mode,
            GameStatus? status,
            DateTime? createdFrom,
            DateTime? createdTo,
            string? sortBy,
            string? sortDirection,
            ISender sender,
            int page = 1,
            int pageSize = 10) =>
        {
            Result<PagedResult<GetAllGamesResponse>> result = await sender.Send(new GetAllGamesQuery(
                userId,
                mode,
                status,
                createdFrom,
                createdTo,
                sortBy ?? "createdAt",
                sortDirection ?? "asc",
                page,
                pageSize));

            return result.IsSuccess ? Results.Ok(result.Value) : ApiResult.Problem(result);
        })
        .WithTags(Tags.Games)
        .WithName("GetAllGames")
        .RequireAuthorization(p => p.RequireRole(Role.Names.Admin));
    }
}
