using Codex.Domain.Entities.Base;

namespace Codex.Domain.Entities;

public sealed class Category : Entity
{
    public string Name { get; private set; }

    public IReadOnlyList<Post> Posts { get; private set; } = [];

    private Category(
        Guid id,
        DateTimeOffset createdAtUtc,
        DateTimeOffset updatedAtUtc,
        string name) : base(id, createdAtUtc, updatedAtUtc) => Name = name;
}