using FluentValidation;

namespace Quiz.Application.UseCases.Users.Disable;

internal sealed class DisableUserCommandValidator : AbstractValidator<DisableUserCommand>
{
    public DisableUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
