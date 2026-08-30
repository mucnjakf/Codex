using Codex.Domain.Entities;

namespace Codex.Application.Dtos.Mappers;

internal static class CommentMapper
{
    extension(Comment comment)
    {
        internal CommentDto ToCommentDto() => new(
            comment.Id,
            comment.CreatedAtUtc,
            comment.UpdatedAtUtc,
            comment.Content,
            comment.Post.ToPostDto(),
            comment.Reader.ToReaderDto());
    }
}