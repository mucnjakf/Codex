using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Mappers;
using Codex.Application.Dtos.Pagination;
using Codex.Application.Mediator;
using Codex.Domain.Entities;
using Codex.Domain.Outcomes;

namespace Codex.Application.Queries.Comments;

public sealed record GetCommentsQuery(int PageNumber, int PageSize)
    : PaginationQueryDto(PageNumber, PageSize), IQuery<PaginationDto<CommentDto>>;

internal sealed class GetCommentsQueryHandler(
    ICommentRepository commentRepository)
    : IQueryHandler<GetCommentsQuery, PaginationDto<CommentDto>>
{
    public async Task<Result<PaginationDto<CommentDto>>> Handle(
        GetCommentsQuery query,
        CancellationToken cancellationToken)
    {
        (IReadOnlyList<Comment> comments, int totalCount) = await commentRepository
            .GetPaginatedAsync(query.PageNumber, query.PageSize, cancellationToken);

        return new PaginationDto<CommentDto>(
            comments.Select(comment => comment.ToCommentDto()).ToList(),
            query.PageNumber,
            query.PageSize,
            totalCount);
    }
}