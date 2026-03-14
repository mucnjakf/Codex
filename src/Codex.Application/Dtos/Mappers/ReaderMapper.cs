using Codex.Domain.Entities;

namespace Codex.Application.Dtos.Mappers;

internal static class ReaderMapper
{
    extension(Reader reader)
    {
        internal ReaderDto ToReaderDto() => new(
            reader.Id,
            reader.CreatedAtUtc,
            reader.UpdatedAtUtc,
            reader.FirstName,
            reader.LastName);
    }
}