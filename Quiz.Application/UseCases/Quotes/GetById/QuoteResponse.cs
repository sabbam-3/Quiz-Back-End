namespace Quiz.Application.UseCases.Quotes.GetById;

public sealed record QuoteResponse(
    Guid Id,
    string Content,
    string AuthorName,
    bool IsActive,
    DateTime CreatedAtUtc);
