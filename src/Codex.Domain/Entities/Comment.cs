using Codex.Domain.Entities.Base;

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
        DateTimeOffset createdAtUtc,
        DateTimeOffset updatedAtUtc,
        string content,
        Guid postId,
        Guid readerId) : base(id, createdAtUtc, updatedAtUtc)
    {
        Content = content;
        PostId = postId;
        ReaderId = readerId;
    }
}