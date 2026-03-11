using Codex.Domain.Entities.Base;
using Codex.Domain.Errors;
using Codex.Domain.Events;
using Codex.Domain.Outcomes;

namespace Codex.Domain.Entities;

public sealed class Category : Entity
{
    public string Name { get; private set; }

    private readonly List<Post> _posts = [];

    public IReadOnlyList<Post> Posts => _posts.AsReadOnly();

    private Category(
        Guid id,
        string name,
        DateTimeOffset createdAtUtc) : base(id, createdAtUtc) => Name = name;

    public static Result<Category> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Category>(CategoryErrors.NameIsRequired);
        }

        Category category = new(Guid.CreateVersion7(), name, DateTimeOffset.UtcNow);

        category.RaiseDomainEvent(new CategoryCreatedDomainEvent(category.Id));

        return Result.Success(category);
    }

    public Result Update(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure(CategoryErrors.NameIsRequired);
        }

        Name = name;

        UpdateUpdatedAtUtc(DateTimeOffset.UtcNow);

        return Result.Success();
    }
}