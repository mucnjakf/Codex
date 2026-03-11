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

    public DateTimeOffset? PublishedAtUtc { get; private set; }

    public Guid AuthorId { get; private set; }

    public Author Author { get; private set; } = null!;

    public Guid CategoryId { get; private set; }

    public Category Category { get; private set; } = null!;

    private readonly List<Comment> _comments = [];

    public IReadOnlyList<Comment> Comments => _comments.AsReadOnly();

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

    public Result Update(string title, string content, Guid categoryId)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            return Result.Failure<Post>(PostErrors.TitleIsRequired);
        }

        if (string.IsNullOrWhiteSpace(content))
        {
            return Result.Failure<Post>(PostErrors.ContentIsRequired);
        }

        if (categoryId == Guid.Empty)
        {
            return Result.Failure<Post>(PostErrors.CategoryIdIsRequired);
        }

        Title = title;
        Content = content;
        CategoryId = categoryId;

        UpdateUpdatedAtUtc(DateTimeOffset.UtcNow);

        return Result.Success();
    }

    public Result Publish()
    {
        if (Status is not PostStatus.Draft)
        {
            return Result.Failure(PostErrors.PublishOnlyDraft);
        }

        Status = PostStatus.Published;

        DateTimeOffset now = DateTimeOffset.UtcNow;
        PublishedAtUtc = now;
        UpdateUpdatedAtUtc(now);

        return Result.Success();
    }
}