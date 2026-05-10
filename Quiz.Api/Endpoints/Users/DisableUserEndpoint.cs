using MediatR;
using Quiz.Api.Abstractions.Endpoints;
using Quiz.Api.Extensions.ApiResults;
using Quiz.Application.UseCases.Users.Disable;
using Quiz.Common.Constants;
using Quiz.Common.Results;
using Quiz.Domain.Roles;

namespace Quiz.Api.Endpoints.Users;

internal sealed class DisableUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("/users/{id:guid}/disable", async (Guid id, ISender sender) =>
        {
            Result result = await sender.Send(new DisableUserCommand(id));

            return result.IsSuccess ? Results.NoContent() : ApiResult.Problem(result);
        })
        .WithTags(Tags.Users)
        .WithName("DisableUser")
        .RequireAuthorization(p => p.RequireRole(Role.Names.Admin));
    }
}