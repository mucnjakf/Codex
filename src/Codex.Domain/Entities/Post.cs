using Codex.Domain.Entities.Base;
using Codex.Domain.Enumerations;

namespace Codex.Domain.Entities;

public sealed class Post(
    Guid id,
    DateTimeOffset createdAtUtc,
    DateTimeOffset updatedAtUtc,
    string title,
    string content,
    Guid authorId,
    Guid categoryId) : Entity(id, createdAtUtc, updatedAtUtc)
{
    public string Title { get; private set; } = title;

    public string Content { get; private set; } = content;

    public PostStatus Status { get; private set; } = PostStatus.Draft;

    public DateTimeOffset? PublishedAtUtc { get; private set; } = null;

    public Guid AuthorId { get; private set; } = authorId;

    public Author Author { get; private set; } = null!;

    public Guid CategoryId { get; private set; } = categoryId;

    public Category Category { get; private set; } = null!;

    public IReadOnlyList<Comment> Comments { get; private set; } = [];
}