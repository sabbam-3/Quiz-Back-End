using Quiz.Application.Abstractions.Messaging;
using Quiz.Application.Abstractions.Repositories;
using Quiz.Common.Results;
using Quiz.Domain.Quotes;

namespace Quiz.Application.UseCases.Quotes.Delete;

internal sealed class DeleteQuoteCommandHandler(
    IQuoteRepository quoteRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteQuoteCommand>
{
    public async Task<Result> Handle(DeleteQuoteCommand command, CancellationToken cancellationToken)
    {
        Quote? quote = await quoteRepository.GetByIdAsync(command.QuoteId, cancellationToken);
        if (quote is null)
        {
            return Result.Failure(QuoteErrors.NotFound(command.QuoteId));
        }

        if (quote.IsDeleted)
        {
            return Result.Failure(QuoteErrors.AlreadyDeleted(command.QuoteId));
        }

        quote.Delete();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
