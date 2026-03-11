using Codex.Application.Dtos.Base;

namespace Codex.Application.Dtos;

public sealed record AuthorDto(
    Guid Id,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc,
    string FirstName,
    string LastName,
    string Biography)
    : EntityDto(Id, CreatedAtUtc, UpdatedAtUtc);