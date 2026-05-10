using MediatR;
using Quiz.Api.Abstractions.Endpoints;
using Quiz.Api.Extensions.ApiResults;
using Quiz.Application.UseCases.Users.GetAll;
using Quiz.Application.UseCases.Users.GetById;
using Quiz.Common.Constants;
using Quiz.Common.Results;
using Quiz.Domain.Roles;

namespace Quiz.Api.Endpoints.Users;

internal sealed class GetAllUsersEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/users", async (
            string? email,
            bool? isActive,
            DateTime? createdFrom,
            DateTime? createdTo,
            string? sortBy,
            string? sortDirection,
            ISender sender,
            int page = 1,
            int pageSize = 10) =>
        {
            Result<PagedResult<UserResponse>> result = await sender.Send(new GetAllUsersQuery(
                email,
                isActive,
                createdFrom,
                createdTo,
                sortBy ?? "createdAt",
                sortDirection ?? "asc",
                page,
                pageSize));

            return result.IsSuccess ? Results.Ok(result.Value) : ApiResult.Problem(result);
        })
        .WithTags(Tags.Users)
        .WithName("GetAllUsers")
        .RequireAuthorization(p => p.RequireRole(Role.Names.Admin));
    }
}
