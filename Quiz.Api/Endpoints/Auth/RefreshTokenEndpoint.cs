using MediatR;
using Quiz.Api.Abstractions.Endpoints;
using Quiz.Api.Extensions.ApiResults;
using Quiz.Application.UseCases.Auth.Login;
using Quiz.Application.UseCases.Auth.RefreshToken;
using Quiz.Common.Constants;
using Quiz.Common.Results;

namespace Quiz.Api.Endpoints.Auth;

internal sealed class RefreshTokenEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/refresh-token", async (RefreshTokenRequest request, ISender sender) =>
        {
            RefreshTokenCommand command = new(request.RefreshToken);

            Result<LoginResponse> result = await sender.Send(command);

            return result.IsSuccess ? Results.Ok(result.Value) : ApiResult.Problem(result);
        })
        .WithTags(Tags.Auth)
        .AllowAnonymous();
    }

    internal sealed record RefreshTokenRequest(string RefreshToken);
}