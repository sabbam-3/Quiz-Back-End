namespace Quiz.Application.UseCases.Users.GetById;

public sealed record UserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    bool IsActive,
    DateTime CreatedAtUtc);
