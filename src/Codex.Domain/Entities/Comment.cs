using Codex.Domain.Entities.Base;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;

namespace Codex.Domain.Entities;

public sealed class Comment : Entity
{
    public string Content { get; private set; }

    public Guid PostId { get; private set; }

    public Post Post { get; private set; } = null!;

    public Guid ReaderId { get; private set; }

    public Reader Reader { get; private set; } = null!;

    private Comment(
        Guid id,
        string content,
        Guid postId,
        Guid readerId,
        DateTimeOffset createdAtUtc) : base(id, createdAtUtc)
    {
        Content = content;
        PostId = postId;
        ReaderId = readerId;
    }

    public static Result<Comment> Create(string content, Guid postId, Guid readerId)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return Result.Failure<Comment>(CommentErrors.ContentIsRequired);
        }

        if (postId == Guid.Empty)
        {
            return Result.Failure<Comment>(CommentErrors.PostIdIsRequired);
        }

        if (readerId == Guid.Empty)
        {
            return Result.Failure<Comment>(CommentErrors.ReaderIdIsRequired);
        }

        Comment comment = new(Guid.CreateVersion7(), content, postId, readerId, DateTimeOffset.UtcNow);

        return Result.Success(comment);
    }
}