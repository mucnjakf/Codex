using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Mappers;
using Codex.Application.Mediator;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;

namespace Codex.Application.Queries.Authors;

public sealed record GetAuthorQuery(Guid Id) : IQuery<AuthorDto>;

internal sealed class GetAuthorQueryHandler(
    IAuthorRepository authorRepository)
    : IQueryHandler<GetAuthorQuery, AuthorDto>
{
    public async Task<Result<AuthorDto>> Handle(GetAuthorQuery query, CancellationToken cancellationToken)
    {
        Author? author = await authorRepository.GetByIdAsync(query.Id, cancellationToken);

        if (author is null)
        {
            return Result.Failure<AuthorDto>(AuthorErrors.NotFound);
        }

        var authorDto = author.ToAuthorDto();

        return Result.Success(authorDto);
    }
}