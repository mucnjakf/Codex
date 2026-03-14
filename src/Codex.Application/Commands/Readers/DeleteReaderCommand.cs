using Codex.Application.Data;
using Codex.Application.Mediator;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;

namespace Codex.Application.Commands.Readers;

public sealed record DeleteReaderCommand(Guid Id) : ICommand;

internal sealed class DeleteReaderCommandHandler(
    IReaderRepository readerRepository,
    ICommentRepository commentRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteReaderCommand>
{
    public async Task<Result> Handle(DeleteReaderCommand command, CancellationToken cancellationToken)
    {
        Reader? reader = await readerRepository.GetByIdAsync(command.Id, cancellationToken);

        if (reader is null)
        {
            return Result.Failure(ReaderErrors.NotFound);
        }

        bool commentsExist = await commentRepository.ExistsByReaderIdAsync(reader.Id, cancellationToken);

        if (commentsExist)
        {
            return Result.Failure(ReaderErrors.CannotDeleteContainsComments);
        }

        readerRepository.Delete(reader);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}