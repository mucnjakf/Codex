using Codex.Domain.Entities;

namespace Codex.Application.Dtos.Mappers;

internal static class AuthorMapper
{
    extension(Author author)
    {
        internal AuthorDto ToAuthorDto() => new(
            author.Id,
            author.CreatedAtUtc,
            author.UpdatedAtUtc,
            author.FirstName,
            author.LastName,
            author.Biography);
    }
}