using MediatR;
using Quiz.Api.Abstractions.Endpoints;
using Quiz.Api.Extensions.ApiResults;
using Quiz.Application.UseCases.Auth.Register;
using Quiz.Common.Constants;
using Quiz.Common.Results;

namespace Quiz.Api.Endpoints.Auth;

internal sealed class RegisterEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/register", async (RegisterRequest request, ISender sender) =>
        {
            RegisterCommand command = new(request.FirstName, request.LastName, request.Email, request.Password);

            Result<Guid> result = await sender.Send(command);

            return result.IsSuccess ? Results.Ok(result.Value) : ApiResult.Problem(result);
        })
        .WithTags(Tags.Auth)
        .AllowAnonymous();
    }

    internal sealed record RegisterRequest(string FirstName, string LastName, string Email, string Password);
}