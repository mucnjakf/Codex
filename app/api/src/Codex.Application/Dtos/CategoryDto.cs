using Codex.Application.Dtos.Base;

namespace Codex.Application.Dtos;

public sealed record CategoryDto(
    Guid Id,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc,
    string Name)
    : EntityDto(Id, CreatedAtUtc, UpdatedAtUtc);