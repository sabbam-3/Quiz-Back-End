using Quiz.Domain.Quotes;

namespace Quiz.Domain.Games;

public sealed class GameQuestion
{
    private GameQuestion() { }

    public Guid Id { get; private set; }

    public Guid GameId { get; private set; }

    public Guid QuoteId { get; private set; }

    public string? AnswerGiven { get; private set; }

    public bool? IsCorrect { get; private set; }

    public bool IsActive { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime? DeletedAtUtc { get; private set; }

    public DateTime? AnsweredAtUtc { get; private set; }

    public string? SuggestedAuthorName { get; private set; }

    public Game Game { get; private set; } = null!;

    public Quote Quote { get; private set; } = null!;

    public bool IsAnswered => AnswerGiven is not null;

    public static GameQuestion CreateBinaryQuestion(Guid gameId, Guid quoteId, string quoteAuthorName, IReadOnlyCollection<string> allAuthorNames)
    {
        bool suggestCorrect = Random.Shared.Next(0, 2) == 1;

        if (suggestCorrect)
        {
            return new GameQuestion
            {
                Id = Guid.NewGuid(),
                GameId = gameId,
                QuoteId = quoteId,
                IsActive = true,
                IsDeleted = false,
                CreatedAtUtc = DateTime.UtcNow,
                SuggestedAuthorName = quoteAuthorName
            };
        }

        string authorName = GetRandomWrongAuthor(quoteAuthorName, allAuthorNames) ?? quoteAuthorName;

        return new GameQuestion
        {
            Id = Guid.NewGuid(),
            GameId = gameId,
            QuoteId = quoteId,
            IsActive = true,
            IsDeleted = false,
            CreatedAtUtc = DateTime.UtcNow,
            SuggestedAuthorName = authorName
        };
    }

    public static GameQuestion CreateMultipleChoiceQuestion(Guid gameId, Guid quoteId)
    {
        return new GameQuestion
        {
            Id = Guid.NewGuid(),
            GameId = gameId,
            QuoteId = quoteId,
            IsActive = true,
            IsDeleted = false,
            CreatedAtUtc = DateTime.UtcNow,
        };
    }

    public void AnswerMultipleChoice(string answerGiven)
    {
        AnswerGiven = answerGiven;
        
        IsCorrect = Quote.AuthorName == answerGiven;

        AnsweredAtUtc = DateTime.UtcNow;
    }

    public void AnswerBinary(string answerGiven)
    {
        AnswerGiven = answerGiven;

        IsCorrect = (answerGiven == "Yes") == (SuggestedAuthorName == Quote.AuthorName);

        AnsweredAtUtc = DateTime.UtcNow;
    }

    public void Delete()
    {
        IsDeleted = true;
        IsActive = false;
        DeletedAtUtc = DateTime.UtcNow;
    }

    public void Abandon()
    {
        IsCorrect = false;
    }

    private static string? GetRandomWrongAuthor(string correctAuthorName, IReadOnlyCollection<string> allAuthorNames)
    {
        return allAuthorNames
            .Where(a => a != correctAuthorName)
            .OrderBy(_ => Random.Shared.Next())
            .FirstOrDefault();
    }
}