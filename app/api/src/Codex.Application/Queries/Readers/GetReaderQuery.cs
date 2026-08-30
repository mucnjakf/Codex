using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Mappers;
using Codex.Application.Mediator;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;

namespace Codex.Application.Queries.Readers;

public sealed record GetReaderQuery(Guid Id) : IQuery<ReaderDto>;

internal sealed class GetReaderQueryHandler(
    IReaderRepository readerRepository)
    : IQueryHandler<GetReaderQuery, ReaderDto>
{
    public async Task<Result<ReaderDto>> Handle(GetReaderQuery query, CancellationToken cancellationToken)
    {
        Reader? reader = await readerRepository.GetByIdAsync(query.Id, cancellationToken);

        if (reader is null)
        {
            return Result.Failure<ReaderDto>(ReaderErrors.NotFound);
        }

        var readerDto = reader.ToReaderDto();

        return Result.Success(readerDto);
    }
}