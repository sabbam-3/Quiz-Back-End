namespace Quiz.Application.Abstractions.Authentication;

public interface IIdentityProviderService
{
    Task<string> LoginAsync(string email, string password, CancellationToken cancellationToken);

    Task<string> RegisterAsync(string email, string password, CancellationToken cancellationToken);
}