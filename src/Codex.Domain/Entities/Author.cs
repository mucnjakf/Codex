using Codex.Domain.Entities.Base;

namespace Codex.Domain.Entities;

public sealed class Author(
    Guid id,
    DateTimeOffset createdAtUtc,
    DateTimeOffset updatedAtUtc,
    string firstName,
    string lastName,
    string biography) : Entity(id, createdAtUtc, updatedAtUtc)
{
    public string FirstName { get; private set; } = firstName;

    public string LastName { get; private set; } = lastName;

    public string Biography { get; private set; } = biography;

    public IReadOnlyList<Post> Posts { get; private set; } = [];
}