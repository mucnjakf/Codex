using Codex.Domain.Entities.Base;
using Codex.Domain.Enumerations;

namespace Codex.Domain.Entities;

public sealed class Post : Entity
{
    public string Title { get; private set; }

    public string Content { get; private set; }

    public PostStatus Status { get; private set; } = PostStatus.Draft;

    public DateTimeOffset? PublishedAtUtc { get; private set; } = null;

    public Guid AuthorId { get; private set; }

    public Author Author { get; private set; } = null!;

    public Guid CategoryId { get; private set; }

    public Category Category { get; private set; } = null!;

    public IReadOnlyList<Comment> Comments { get; private set; } = [];

    private Post(
        Guid id,
        DateTimeOffset createdAtUtc,
        DateTimeOffset updatedAtUtc,
        string title,
        string content,
        Guid authorId,
        Guid categoryId) : base(id, createdAtUtc, updatedAtUtc)
    {
        Title = title;
        Content = content;
        AuthorId = authorId;
        CategoryId = categoryId;
    }
}