using Codex.Domain.Entities.Base;

namespace Codex.Domain.Entities;

public sealed class Category(
    Guid id,
    DateTimeOffset createdAtUtc,
    DateTimeOffset updatedAtUtc,
    string name) : Entity(id, createdAtUtc, updatedAtUtc)
{
    public string Name { get; private set; } = name;

    public IReadOnlyList<Post> Posts { get; private set; } = [];
}