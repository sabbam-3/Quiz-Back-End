using FluentValidation;

namespace Quiz.Application.UseCases.Quotes.Update;

internal sealed class UpdateQuoteCommandValidator : AbstractValidator<UpdateQuoteCommand>
{
    public UpdateQuoteCommandValidator()
    {
        RuleFor(x => x.QuoteId)
            .NotEmpty();

        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.AuthorName)
            .NotEmpty()
            .MaximumLength(200);
    }
}
