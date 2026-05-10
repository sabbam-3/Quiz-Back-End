using MediatR;
using Quiz.Api.Abstractions.Endpoints;
using Quiz.Api.Extensions.ApiResults;
using Quiz.Application.UseCases.Users.GetByGames;
using Quiz.Common.Constants;
using Quiz.Common.Results;
using Quiz.Domain.Roles;

namespace Quiz.Api.Endpoints.Users;

internal sealed class GetGamesByUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/{userId:guid}/games", async (Guid userId, ISender sender) =>
        {
            Result<IReadOnlyCollection<GetGamesByUserResponse>> result =
                await sender.Send(new GetGamesByUserQuery(userId));

            return result.IsSuccess ? Results.Ok(result.Value) : ApiResult.Problem(result);
        })
        .WithTags(Tags.Games)
        .WithName("GetGamesByUser")
        .RequireAuthorization(p => p.RequireRole(Role.Names.Admin));
    }
}