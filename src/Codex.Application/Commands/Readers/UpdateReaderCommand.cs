using Codex.Application.Data;
using Codex.Application.Mediator;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;

namespace Codex.Application.Commands.Readers;

public sealed record UpdateReaderCommand(Guid Id, string FirstName, string LastName) : ICommand;

internal sealed class UpdateReaderCommandHandler(
    IReaderRepository readerRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateReaderCommand>
{
    public async Task<Result> Handle(UpdateReaderCommand command, CancellationToken cancellationToken)
    {
        Reader? reader = await readerRepository.GetByIdAsync(command.Id, cancellationToken);

        if (reader is null)
        {
            return Result.Failure(ReaderErrors.NotFound);
        }

        Result readerUpdateResult = reader.Update(command.FirstName, command.LastName);

        if (readerUpdateResult.IsFailure)
        {
            return Result.Failure(readerUpdateResult.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}