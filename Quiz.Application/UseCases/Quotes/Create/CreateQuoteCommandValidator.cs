using FluentValidation;

namespace Quiz.Application.UseCases.Quotes.Create;

internal sealed class CreateQuoteCommandValidator : AbstractValidator<CreateQuoteCommand>
{
    public CreateQuoteCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.AuthorName)
            .NotEmpty()
            .MaximumLength(200);
    }
}
