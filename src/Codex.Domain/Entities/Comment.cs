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
        string content,
        Guid postId,
        Guid readerId,
        DateTimeOffset createdAtUtc) : base(id, createdAtUtc)
    {
        Content = content;
        PostId = postId;
        ReaderId = readerId;
    }
}