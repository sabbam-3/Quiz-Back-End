using FluentValidation;

namespace Quiz.Application.UseCases.Games.AnswerMultipleChoiceQuestion;

internal sealed class AnswerMultipleChoiceQuestionCommandValidator : AbstractValidator<AnswerMultipleChoiceQuestionCommand>
{
    public AnswerMultipleChoiceQuestionCommandValidator()
    {
        RuleFor(x => x.GameId).NotEmpty();
        RuleFor(x => x.Answer).NotEmpty().MaximumLength(200);
    }
}
