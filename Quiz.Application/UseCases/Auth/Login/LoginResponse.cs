namespace Quiz.Application.UseCases.Auth.Login;

public sealed record LoginResponse(string AccessToken, string RefreshToken);
