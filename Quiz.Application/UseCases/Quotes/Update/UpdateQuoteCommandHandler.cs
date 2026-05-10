using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Common.Results;
using Quiz.Domain.Quotes;

namespace Quiz.Application.UseCases.Quotes.Update;

internal sealed class UpdateQuoteCommandHandler(
    IQuoteRepository quoteRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateQuoteCommand>
{
    public async Task<Result> Handle(UpdateQuoteCommand command, CancellationToken cancellationToken)
    {
        Quote? quote = await quoteRepository.GetByIdAsync(command.QuoteId, cancellationToken);
        if (quote is null)
        {
            return Result.Failure(QuoteErrors.NotFound(command.QuoteId));
        }

        if (!quote.IsSameAs(command.AuthorName, command.Content))
        {
            bool exists = await quoteRepository.ExistsByAuthorNameAndContentAsync(
                command.AuthorName, command.Content, cancellationToken);

            if (exists)
            {
                return Result.Failure(QuoteErrors.DuplicateQuote(command.AuthorName, command.Content));
            }
        }

        quote.Update(command.Content, command.AuthorName);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}