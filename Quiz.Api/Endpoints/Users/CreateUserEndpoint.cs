using MediatR;
using Quiz.Api.Abstractions.Endpoints;
using Quiz.Api.Extensions.ApiResults;
using Quiz.Application.UseCases.Users.Create;
using Quiz.Common.Constants;
using Quiz.Common.Results;
using Quiz.Domain.Roles;

namespace Quiz.Api.Endpoints.Users;

internal sealed class CreateUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/users", async (CreateUserRequest request, ISender sender) =>
        {
            CreateUserCommand command = new(request.FirstName, request.LastName, request.Email, request.Password, request.Role);

            Result<Guid> result = await sender.Send(command);

            return result.IsSuccess
                ? Results.CreatedAtRoute("GetUserById", new { id = result.Value }, result.Value)
                : ApiResult.Problem(result);
        })
        .WithTags(Tags.Users)
        .WithName("CreateUser")
        .RequireAuthorization(p => p.RequireRole(Role.Names.Admin));
    }

    internal sealed record CreateUserRequest(string FirstName, string LastName, string Email, string Password, string Role);
}
