using Quiz.Application.Abstractions.Authentication;
using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Common.Results;
using Quiz.Domain.Games;
using Quiz.Domain.Quotes;

namespace Quiz.Application.UseCases.Games.GetMultipleChoiceQuestions;

internal sealed class GetMultipleChoiceQuestionsQueryHandler(
    IUserContext userContext,
    IQuoteRepository quoteRepository,
    IGameRepository gameRepository) : IQueryHandler<GetMultipleChoiceQuestionsQuery, GetMultipleChoiceQuestionsResponse>
{
    public async Task<Result<GetMultipleChoiceQuestionsResponse>> Handle(GetMultipleChoiceQuestionsQuery query, CancellationToken cancellationToken)
    {
        Game? game = await gameRepository.GetByIdWithUnansweredQuestionsAsync(query.GameId, cancellationToken);
        if (game is null)
        {
            return Result.Failure<GetMultipleChoiceQuestionsResponse>(GameErrors.NotFound(query.GameId));
        }

        if (game.UserId != userContext.UserId)
        {
            return Result.Failure<GetMultipleChoiceQuestionsResponse>(GameErrors.Unauthorized(query.GameId));
        }

        if (game.IsBinary)
        {
            return Result.Failure<GetMultipleChoiceQuestionsResponse>(GameErrors.IncorrectQuizMode(query.GameId));
        }

        if (game.IsCompleted)
        {
            return Result.Failure<GetMultipleChoiceQuestionsResponse>(GameErrors.AlreadyCompleted(query.GameId));
        }

        GameQuestion? current = game.Questions.FirstOrDefault(q => !q.IsAnswered);
        if (current is null)
        {
            return Result.Failure<GetMultipleChoiceQuestionsResponse>(GameErrors.NoCurrentQuestion(query.GameId));
        }

        List<MultipleChoiceQuestionResponse> questions = new();

        IReadOnlyCollection<Quote> allQuotes = await quoteRepository.GetActiveQuotesAsync(20, cancellationToken: cancellationToken);

        foreach (var question in game.Questions)
        {
            string correctAuthor = question.Quote.AuthorName;

            List<string> wrongAuthors = allQuotes
                .Where(q => q.AuthorName != correctAuthor)
                .Select(q => q.AuthorName)
                .Distinct()
                .OrderBy(_ => Guid.NewGuid())
                .Take(2)
                .ToList();

            List<string> options = [correctAuthor, .. wrongAuthors];
            options = [.. options.OrderBy(_ => Guid.NewGuid())];

            var multipleChoiceQuestion = new MultipleChoiceQuestionResponse(question.Id, question.Quote.Content, question.IsAnswered, options);

            questions.Add(multipleChoiceQuestion);
        }

        return new GetMultipleChoiceQuestionsResponse(game.Id, questions);
    }
}