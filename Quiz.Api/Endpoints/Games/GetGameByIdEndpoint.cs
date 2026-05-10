using MediatR;
using Quiz.Api.Abstractions.Endpoints;
using Quiz.Api.Extensions.ApiResults;
using Quiz.Application.UseCases.Games.GetById;
using Quiz.Common.Constants;
using Quiz.Common.Results;
using Quiz.Domain.Roles;

namespace Quiz.Api.Endpoints.Games;

internal sealed class GetGameByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/games/{id:guid}", async (Guid id, ISender sender) =>
        {
            Result<GetGameByIdResponse> result = await sender.Send(new GetGameByIdQuery(id));

            return result.IsSuccess ? Results.Ok(result.Value) : ApiResult.Problem(result);
        })
        .WithTags(Tags.Games)
        .WithName("GetGameById")
        .RequireAuthorization(p => p.RequireRole(Role.Names.User, Role.Names.Admin));
    }
}
