using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Mappers;
using Codex.Application.Dtos.Pagination;
using Codex.Application.Mediator;
using Codex.Domain.Entities;
using Codex.Domain.Outcomes;

namespace Codex.Application.Queries.Readers;

public sealed record GetReadersQuery(int PageNumber, int PageSize)
    : PaginationQueryDto(PageNumber, PageSize), IQuery<PaginationDto<ReaderDto>>;

internal sealed class GetReadersQueryHandler(
    IReaderRepository readerRepository)
    : IQueryHandler<GetReadersQuery, PaginationDto<ReaderDto>>
{
    public async Task<Result<PaginationDto<ReaderDto>>> Handle(
        GetReadersQuery query,
        CancellationToken cancellationToken)
    {
        (IReadOnlyList<Reader> readers, int totalCount) = await readerRepository
            .GetPaginatedAsync(query.PageNumber, query.PageSize, cancellationToken);

        return new PaginationDto<ReaderDto>(
            readers.Select(reader => reader.ToReaderDto()).ToList(),
            query.PageNumber,
            query.PageSize,
            totalCount);
    }
}