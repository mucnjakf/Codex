using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Mappers;
using Codex.Application.Mediator;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;

namespace Codex.Application.Queries.Comments;

public sealed record GetCommentQuery(Guid Id) : IQuery<CommentDto>;

internal sealed class GetCommentQueryHandler(
    ICommentRepository commentRepository)
    : IQueryHandler<GetCommentQuery, CommentDto>
{
    public async Task<Result<CommentDto>> Handle(GetCommentQuery query, CancellationToken cancellationToken)
    {
        Comment? comment = await commentRepository.GetByIdAsync(query.Id, cancellationToken);

        if (comment is null)
        {
            return Result.Failure<CommentDto>(CommentErrors.NotFound);
        }

        var commentDto = comment.ToCommentDto();

        return Result.Success(commentDto);
    }
}