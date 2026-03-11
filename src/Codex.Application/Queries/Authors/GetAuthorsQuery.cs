using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Mappers;
using Codex.Application.Dtos.Pagination;
using Codex.Application.Mediator;
using Codex.Domain.Entities;
using Codex.Domain.Outcomes;

namespace Codex.Application.Queries.Authors;

public sealed record GetAuthorsQuery(int PageNumber, int PageSize) : IQuery<PaginationDto<AuthorDto>>;

internal sealed class GetAuthorsQueryHandler(
    IAuthorRepository authorRepository)
    : IQueryHandler<GetAuthorsQuery, PaginationDto<AuthorDto>>
{
    public async Task<Result<PaginationDto<AuthorDto>>> Handle(
        GetAuthorsQuery query,
        CancellationToken cancellationToken)
    {
        (IReadOnlyList<Author> authors, int totalCount) = await authorRepository
            .GetPaginatedAsNoTrackingAsync(query.PageNumber, query.PageSize, cancellationToken);

        return new PaginationDto<AuthorDto>(
            authors.Select(author => author.ToAuthorDto()).ToList(),
            query.PageNumber,
            query.PageSize,
            totalCount);
    }
}