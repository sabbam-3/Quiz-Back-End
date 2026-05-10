using MediatR;
using Quiz.Api.Abstractions.Endpoints;
using Quiz.Api.Extensions.ApiResults;
using Quiz.Application.UseCases.Auth.Login;
using Quiz.Common.Constants;
using Quiz.Common.Results;

namespace Quiz.Api.Endpoints.Auth;

internal sealed class LoginEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/login", async (LoginRequest request, ISender sender) =>
        {
            LoginCommand command = new(request.Email, request.Password);

            Result<LoginResponse> result = await sender.Send(command);

            return result.IsSuccess ? Results.Ok(result.Value) : ApiResult.Problem(result);
        })
        .WithTags(Tags.Auth)
        .AllowAnonymous();
    }

    internal sealed record LoginRequest(string Email, string Password);
}