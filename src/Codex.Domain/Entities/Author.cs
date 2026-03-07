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
        string firstName,
        string lastName,
        string biography,
        DateTimeOffset createdAtUtc) : base(id, createdAtUtc)
    {
        FirstName = firstName;
        LastName = lastName;
        Biography = biography;
    }
}