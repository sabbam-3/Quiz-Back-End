using MediatR;
using Quiz.Api.Abstractions.Endpoints;
using Quiz.Api.Extensions.ApiResults;
using Quiz.Application.UseCases.Users.Delete;
using Quiz.Common.Constants;
using Quiz.Common.Results;
using Quiz.Domain.Roles;

namespace Quiz.Api.Endpoints.Users;

internal sealed class DeleteUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/users/{id:guid}", async (Guid id, ISender sender) =>
        {
            Result result = await sender.Send(new DeleteUserCommand(id));

            return result.IsSuccess ? Results.NoContent() : ApiResult.Problem(result);
        })
        .WithTags(Tags.Users)
        .WithName("DeleteUser")
        .RequireAuthorization(p => p.RequireRole(Role.Names.Admin));
    }
}
