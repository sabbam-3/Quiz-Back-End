using FluentValidation;

namespace Quiz.Application.UseCases.Games.AnswerBinaryQuestion;

internal sealed class AnswerBinaryQuestionCommandValidator : AbstractValidator<AnswerBinaryQuestionCommand>
{
    public AnswerBinaryQuestionCommandValidator()
    {
        RuleFor(x => x.GameId).NotEmpty();
        RuleFor(x => x.Answer).NotEmpty().Must(a => a is "Yes" or "No")
            .WithMessage("Answer must be 'Yes' or 'No'.");
    }
}