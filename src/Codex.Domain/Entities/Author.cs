using Codex.Domain.Entities.Base;

namespace Codex.Domain.Entities;

public sealed class Author : Entity
{
    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string Biography { get; private set; }

    public IReadOnlyList<Post> Posts { get; private set; } = [];

    private Author(
        Guid id,
        DateTimeOffset createdAtUtc,
        DateTimeOffset updatedAtUtc,
        string firstName,
        string lastName,
        string biography) : base(id, createdAtUtc, updatedAtUtc)
    {
        FirstName = firstName;
        LastName = lastName;
        Biography = biography;
    }
}