using Microsoft.AspNetCore.Identity;
using Quiz.Application.Abstractions.Authentication;
using Quiz.Common.Exceptions;
using Quiz.Domain.Identities;

namespace Quiz.Infrastructure.Authentication;

internal sealed class IdentityProviderService(
    UserManager<IdentityUser<string>> userManager) : IIdentityProviderService
{
    public async Task<string> LoginAsync(string email, string password, CancellationToken cancellationToken)
    {
        IdentityUser<string>? identityUser = await userManager.FindByEmailAsync(email);
        if (identityUser is null)
        {
            throw new UnauthorizedAccessException();
        }

        bool passwordValid = await userManager.CheckPasswordAsync(identityUser, password);
        if (!passwordValid)
        {
            throw new UnauthorizedAccessException();
        }

        return identityUser.Id;
    }

    public async Task<string> RegisterAsync(string email, string password, CancellationToken cancellationToken)
    {
        IdentityUser<string> identity = new()
        {
            Id = Guid.NewGuid().ToString(),
            Email = email,
            UserName = email
        };

        IdentityResult result = await userManager.CreateAsync(identity, password);
        if (!result.Succeeded)
        {
            string description = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new QuizException(nameof(IIdentityProviderService), IdentityErrors.RegistrationFailed(description));
        }

        return identity.Id;
    }
}