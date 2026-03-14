using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Mappers;
using Codex.Application.Mediator;
using Codex.Domain.Entities;
using Codex.Domain.Outcomes;

namespace Codex.Application.Commands.Readers;

public sealed record CreateReaderCommand(string FirstName, string LastName) : ICommand<ReaderDto>;

internal sealed class CreateReaderCommandHandler(
    IReaderRepository readerRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateReaderCommand, ReaderDto>
{
    public async Task<Result<ReaderDto>> Handle(CreateReaderCommand command, CancellationToken cancellationToken)
    {
        Result<Reader> readerCreateResult = Reader.Create(
            command.FirstName,
            command.LastName);

        if (readerCreateResult.IsFailure)
        {
            return Result.Failure<ReaderDto>(readerCreateResult.Error);
        }

        await readerRepository.CreateAsync(readerCreateResult.Value, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var readerDto = readerCreateResult.Value.ToReaderDto();

        return Result.Success(readerDto);
    }
}