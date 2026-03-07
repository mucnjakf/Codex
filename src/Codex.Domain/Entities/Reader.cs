using Codex.Domain.Entities.Base;

namespace Codex.Domain.Entities;

public sealed class Reader : Entity
{
    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public IReadOnlyList<Comment> Comments { get; private set; } = [];

    private Reader(
        Guid id,
        string firstName,
        string lastName,
        DateTimeOffset createdAtUtc) : base(id, createdAtUtc)
    {
        FirstName = firstName;
        LastName = lastName;
    }
}