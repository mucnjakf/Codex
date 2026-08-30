using Codex.Application.Dtos.Base;

namespace Codex.Application.Dtos;

public sealed record ReaderDto(
    Guid Id,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc,
    string FirstName,
    string LastName)
    : EntityDto(Id, CreatedAtUtc, UpdatedAtUtc);