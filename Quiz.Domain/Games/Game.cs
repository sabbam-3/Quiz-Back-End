using Quiz.Common.Results;
using Quiz.Domain.Quotes;
using Quiz.Domain.Users;

namespace Quiz.Domain.Games;

public sealed class Game
{
    private readonly List<GameQuestion> _questions = [];

    private Game() { }

    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public QuizMode Mode { get; private set; }

    public GameStatus Status { get; private set; }

    public bool IsActive { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime? DeletedAtUtc { get; private set; }

    public float Score { get; private set; }

    public User User { get; private set; } = null!;

    public bool IsCompleted => Status == GameStatus.Completed;

    public bool IsBinary => Mode == QuizMode.Binary;

    public bool IsMultipleChoice => Mode == QuizMode.MultipleChoice;

    public bool IsAllQuestionsAnswered => Questions.All(q => q.IsAnswered);

    public IReadOnlyCollection<GameQuestion> Questions => _questions.AsReadOnly();

    public static Result<Game> CreateBinary(Guid userId, IReadOnlyCollection<Quote> quotes, IReadOnlyCollection<string> allAuthorNames)
    {
        if (quotes.Count == 0)
        {
            return Result.Failure<Game>(GameErrors.NoQuotesAvailable());
        }

        if (allAuthorNames.Count < 3)
        {
            return Result.Failure<Game>(GameErrors.NotEnoughAuthors());
        }

        var game = new Game
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Mode = QuizMode.Binary,
            Status = GameStatus.InProgress,
            IsActive = true,
            IsDeleted = false,
            CreatedAtUtc = DateTime.UtcNow
        };

        foreach (Quote quote in quotes)
        {
            GameQuestion question = GameQuestion.CreateBinaryQuestion(game.Id, quote.Id, quote.AuthorName, allAuthorNames);
            game.AddQuestion(question);
        }

        return game;
    }

    public static Result<Game> CreateMultipleChoice(Guid userId, IReadOnlyCollection<Quote> quotes)
    {
        if (quotes.Count == 0)
        {
            return Result.Failure<Game>(GameErrors.NoQuotesAvailable());
        }

        if (quotes.Count < 3)
        {
            return Result.Failure<Game>(QuoteErrors.InsufficientQuotesForMultipleChoice());
        }

        var game = new Game
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Mode = QuizMode.MultipleChoice,
            Status = GameStatus.InProgress,
            IsActive = true,
            IsDeleted = false,
            CreatedAtUtc = DateTime.UtcNow
        };

        foreach (Quote quote in quotes)
        {
            GameQuestion question = GameQuestion.CreateMultipleChoiceQuestion(userId, quote.Id);

            game.AddQuestion(question);
        }

        return game;
    }

    public void Complete()
    {
        Score = CalculateScore();

        Status = GameStatus.Completed;
        IsActive = false;
    }

    public void Delete()
    {
        IsDeleted = true;
        IsActive = false;
        DeletedAtUtc = DateTime.UtcNow;
    }

    public void AddQuestion(GameQuestion question)
    {
        _questions.Add(question);
    }

    public void AbandonAllUnansweredQuestions()
    {
        foreach (var question in Questions)
        {
            if (!question.IsAnswered)
            {
                question.Abandon();
            }
        }

        Score = CalculateScore();
        Status = GameStatus.Abandoned;
        IsActive = false;
    }

    private float CalculateScore()
    {
        int correctQuestions = Questions.Where(q => q.IsCorrect == true).ToList().Count;
        return (float)correctQuestions / Questions.Count() * 100;
    }
}