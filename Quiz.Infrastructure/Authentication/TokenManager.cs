using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Quiz.Application.Abstractions.Authentication;
using Quiz.Application.Extensions.Authentication;
using Quiz.Application.Models;
using Quiz.Common.Exceptions;
using Quiz.Domain.Identities;
using Quiz.Infrastructure.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Quiz.Infrastructure.Authentication;

internal sealed class TokenManager(UserManager<IdentityUser<string>> userManager, IOptions<JwtConfiguration> configuration) : ITokenManager
{
    private const string LoginProvider = "Quiz";
    private const string TokenName = "RefreshToken";
    public async Task<TokenResponse> GenerateTokensAsync(Guid userId, string identityId, string roleName, CancellationToken cancellationToken)
    {
        string refreshToken = Guid.NewGuid().ToString("N");

        await SetAuthenticationTokenAsync(identityId, refreshToken, cancellationToken);

        string accessToken = GenerateJwtToken(userId, roleName);

        return new TokenResponse(accessToken, refreshToken);
    }

    public async Task<string?> GetAuthenticationTokenAsync(string identityId, CancellationToken cancellationToken = default)
    {
        IdentityUser<string>? identityUser = await userManager.FindByIdAsync(identityId);
        if (identityUser is null)
        {
            throw new QuizException(nameof(ITokenManager), IdentityErrors.NotFound(identityId));
        }

        return await userManager.GetAuthenticationTokenAsync(identityUser, LoginProvider, TokenName);
    }

    private async Task SetAuthenticationTokenAsync(string identityId, string token, CancellationToken cancellationToken = default)
    {
        IdentityUser<string>? identityUser = await userManager.FindByIdAsync(identityId);
        if (identityUser is null)
        {
            throw new QuizException(nameof(ITokenManager), IdentityErrors.NotFound(identityId));
        }

        await userManager.SetAuthenticationTokenAsync(identityUser, LoginProvider, TokenName, token);
    }

    private string GenerateJwtToken(Guid userId, string role)
    {
        Claim[] claims =
        [
            new Claim(CustomClaims.UserId, userId.ToString()),
            new Claim(CustomClaims.Role, role)
        ];

        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(configuration.Value.Secret));
        SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new(
            issuer: configuration.Value.Issuer,
            audience: configuration.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(configuration.Value.ExpirationHours),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}