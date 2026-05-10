namespace Quiz.Infrastructure.Configurations;

internal class JwtConfiguration
{
    public required string Secret { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public int ExpirationHours { get; init; } = 1;
}