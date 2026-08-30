using Codex.Domain.Entities.Base;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;

namespace Codex.Domain.Entities;

public sealed class Author : Entity
{
    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public string Biography { get; private set; }

    private readonly List<Post> _posts = [];

    public IReadOnlyList<Post> Posts => _posts.AsReadOnly();

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

    public static Result<Author> Create(string firstName, string lastName, string biography)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result.Failure<Author>(AuthorErrors.FirstNameIsRequired);
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            return Result.Failure<Author>(AuthorErrors.LastNameIsRequired);
        }

        if (string.IsNullOrWhiteSpace(biography))
        {
            return Result.Failure<Author>(AuthorErrors.BiographyIsRequired);
        }

        Author author = new(Guid.CreateVersion7(), firstName, lastName, biography, DateTimeOffset.UtcNow);

        return Result.Success(author);
    }

    public Result Update(string firstName, string lastName, string biography)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result.Failure<Author>(AuthorErrors.FirstNameIsRequired);
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            return Result.Failure<Author>(AuthorErrors.LastNameIsRequired);
        }

        if (string.IsNullOrWhiteSpace(biography))
        {
            return Result.Failure<Author>(AuthorErrors.BiographyIsRequired);
        }

        FirstName = firstName;
        LastName = lastName;
        Biography = biography;
        UpdatedAtUtc = DateTimeOffset.UtcNow;

        return Result.Success();
    }
}