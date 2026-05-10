using Quiz.Common.Results;

namespace Quiz.Domain.Quotes;

public sealed class Quote
{
    private Quote() { }

    public Guid Id { get; private set; }

    public string Content { get; private set; } = string.Empty;

    public string AuthorName { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

    public bool IsDeleted { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime? DeletedAtUtc { get; private set; }

    public static Result<Quote> Create(string content, string authorName)
    {
        return new Quote
        {
            Id = Guid.NewGuid(),
            Content = content,
            AuthorName = authorName,
            IsActive = true,
            IsDeleted = false,
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    public void Update(string content, string authorName)
    {
        Content = content;
        AuthorName = authorName;
    }

    public void Delete()
    {
        IsDeleted = true;
        IsActive = false;
        DeletedAtUtc = DateTime.UtcNow;
    }

    public bool IsSameAs(string authorName, string content) =>
        string.Equals(AuthorName, authorName, StringComparison.OrdinalIgnoreCase) &&
        string.Equals(Content, content, StringComparison.OrdinalIgnoreCase);
}