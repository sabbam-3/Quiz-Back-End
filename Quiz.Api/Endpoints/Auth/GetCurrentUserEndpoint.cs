using MediatR;
using Quiz.Api.Abstractions.Endpoints;
using Quiz.Api.Extensions.ApiResults;
using Quiz.Application.UseCases.Auth.GetCurrentUser;
using Quiz.Common.Constants;
using Quiz.Domain.Roles;

namespace Quiz.Api.Endpoints.Auth;

internal sealed class GetCurrentUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/auth/user", async (ISender sender) =>
        {
            var result = await sender.Send(new GetCurrentUserQuery());

            return result.IsSuccess ? Results.Ok(result.Value) : ApiResult.Problem(result);
        })
        .WithTags(Tags.Auth)
        .RequireAuthorization(p => p.RequireRole(Role.Names.User, Role.Names.Admin));
    }
}