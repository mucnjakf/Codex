using Codex.Domain.Entities.Base;

namespace Codex.Domain.Entities;

public sealed class Reader(
    Guid id,
    DateTimeOffset createdAtUtc,
    DateTimeOffset updatedAtUtc,
    string firstName,
    string lastName) : Entity(id, createdAtUtc, updatedAtUtc)
{
    public string FirstName { get; private set; } = firstName;

    public string LastName { get; private set; } = lastName;

    public IReadOnlyList<Comment> Comments { get; private set; } = [];
}