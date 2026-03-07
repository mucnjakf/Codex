using Codex.Domain.Entities.Base;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;

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

    public static Result<Reader> Create(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result.Failure<Reader>(ReaderErrors.FirstNameIsRequired);
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            return Result.Failure<Reader>(ReaderErrors.LastNameIsRequired);
        }

        Reader reader = new(Guid.CreateVersion7(), firstName, lastName, DateTimeOffset.UtcNow);

        return Result.Success(reader);
    }

    public Result Update(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result.Failure(ReaderErrors.FirstNameIsRequired);
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            return Result.Failure(ReaderErrors.LastNameIsRequired);
        }

        FirstName = firstName;
        LastName = lastName;

        UpdateUpdatedAtUtc(DateTimeOffset.UtcNow);

        return Result.Success();
    }
}