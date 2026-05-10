using Quiz.Common.Results;
using Quiz.Domain.Games;

namespace Quiz.Domain.Users;

public sealed class User
{
    private User() { }

    public Guid Id { get; set; }

    public string FirstName { get; private set; } = default!;

    public string LastName { get; private set; } = default!;

    public string Email { get; set; } = default!;

    public string IdentityId { get; private set; } = default!;

    public bool IsActive { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime? DeletedAtUtc { get; private set; }

    public ICollection<Game> Games { get; private set; } = [];

    public static Result<User> Create(string firstName, string lastName, string email, string identityId)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            IdentityId = identityId,
            Email = email,
            IsActive = true,
            IsDeleted = false,
            CreatedAtUtc = DateTime.UtcNow,
        };
    }

    public void Update(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public void Disable()
    {
        IsActive = false;
    }

    public void Enable()
    {
        IsActive = true;
    }

    public void Delete()
    {
        IsDeleted = true;
        IsActive = false;
        DeletedAtUtc = DateTime.UtcNow;
    }
}