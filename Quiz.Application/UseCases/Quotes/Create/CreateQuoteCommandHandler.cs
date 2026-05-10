using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Common.Results;
using Quiz.Domain.Quotes;

namespace Quiz.Application.UseCases.Quotes.Create;

internal sealed class CreateQuoteCommandHandler(
    IQuoteRepository quoteRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateQuoteCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateQuoteCommand command, CancellationToken cancellationToken)
    {
        bool exists = await quoteRepository.ExistsByAuthorNameAndContentAsync(
            command.AuthorName, command.Content, cancellationToken);

        if (exists)
        {
            return Result.Failure<Guid>(QuoteErrors.DuplicateQuote(command.AuthorName, command.Content));
        }

        Result<Quote> quote = Quote.Create(command.Content, command.AuthorName);

        if (quote.IsFailure)
        {
            return Result.Failure<Guid>(quote.Error);
        }

        await quoteRepository.AddAsync(quote.Value, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(quote.Value.Id);
    }
}