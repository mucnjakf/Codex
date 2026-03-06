using Codex.Domain.Entities.Base;

namespace Codex.Domain.Entities;

public sealed class Reader : Entity
{
    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public IReadOnlyList<Comment> Comments { get; private set; } = [];

    private Reader(
        Guid id,
        DateTimeOffset createdAtUtc,
        DateTimeOffset updatedAtUtc,
        string firstName,
        string lastName) : base(id, createdAtUtc, updatedAtUtc)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}