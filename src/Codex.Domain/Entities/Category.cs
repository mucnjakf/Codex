using Codex.Domain.Entities.Base;
using Codex.Domain.Errors;
using Codex.Domain.Events;
using Codex.Domain.Outcomes;

namespace Codex.Domain.Entities;

public sealed class Category : Entity
{
    public string Name { get; private set; }

    public IReadOnlyList<Post> Posts { get; private set; } = [];

    private Category(
        Guid id,
        string name,
        DateTimeOffset createdAtUtc) : base(id, createdAtUtc) => Name = name;

    public static Result<Category> Create(string name, DateTimeOffset createdAt)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Category>(CategoryErrors.NameIsRequired);
        }

        Category category = new(Guid.CreateVersion7(), name, createdAt);

        category.RaiseDomainEvent(new CategoryCreatedDomainEvent(category.Id));

        return Result.Success(category);
    }
}