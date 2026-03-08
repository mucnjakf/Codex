using Codex.Domain.Entities.Base;
using Codex.Domain.Enumerations;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;

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
        string title,
        string content,
        Guid authorId,
        Guid categoryId,
        DateTimeOffset createdAtUtc) : base(id, createdAtUtc)
    {
        Title = title;
        Content = content;
        AuthorId = authorId;
        CategoryId = categoryId;
    }

    public static Result<Post> Create(string title, string content, Guid authorId, Guid categoryId)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return Result.Failure<Post>(PostErrors.TitleIsRequired);
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            return Result.Failure<Post>(PostErrors.ContentIsRequired);
        }

        if (authorId == Guid.Empty)
        {
            return Result.Failure<Post>(PostErrors.AuthorIdIsRequired);
        }

        if (categoryId == Guid.Empty)
        {
            return Result.Failure<Post>(PostErrors.CategoryIdIsRequired);
        }

        Post post = new(Guid.CreateVersion7(), title, content, authorId, categoryId, DateTimeOffset.UtcNow);

        return Result.Success(post);
    }
}