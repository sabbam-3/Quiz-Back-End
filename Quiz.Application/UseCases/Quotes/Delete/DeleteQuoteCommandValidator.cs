using FluentValidation;

namespace Quiz.Application.UseCases.Quotes.Delete;

internal sealed class DeleteQuoteCommandValidator : AbstractValidator<DeleteQuoteCommand>
{
    public DeleteQuoteCommandValidator()
    {
        RuleFor(x => x.QuoteId)
            .NotEmpty();
    }
}
