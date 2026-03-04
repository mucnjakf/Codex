using Codex.Domain.Entities.Base;

namespace Codex.Domain.Entities;

public sealed class Comment(
    Guid id,
    DateTimeOffset createdAtUtc,
    DateTimeOffset updatedAtUtc,
    string content,
    Guid postId,
    Guid readerId) : Entity(id, createdAtUtc, updatedAtUtc)
{
    public string Content { get; private set; } = content;

    public Guid PostId { get; private set; } = postId;

    public Post Post { get; private set; } = null!;

    public Guid ReaderId { get; private set; } = readerId;

    public Reader Reader { get; private set; } = null!;
}