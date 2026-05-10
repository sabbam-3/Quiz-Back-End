using MediatR;
using Quiz.Api.Abstractions.Endpoints;
using Quiz.Api.Extensions.ApiResults;
using Quiz.Application.UseCases.Users.Update;
using Quiz.Common.Constants;
using Quiz.Common.Results;
using Quiz.Domain.Roles;

namespace Quiz.Api.Endpoints.Users;

internal sealed class UpdateUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/users/{id:guid}", async (Guid id, UpdateUserRequest request, ISender sender) =>
        {
            UpdateUserCommand command = new(id, request.FirstName, request.LastName, request.Email);

            Result result = await sender.Send(command);

            return result.IsSuccess ? Results.NoContent() : ApiResult.Problem(result);
        })
        .WithTags(Tags.Users)
        .WithName("UpdateUser")
        .RequireAuthorization(p => p.RequireRole(Role.Names.Admin));
    }

    internal sealed record UpdateUserRequest(string FirstName, string LastName, string Email);
}