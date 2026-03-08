using Codex.Application.Dtos.Base;

namespace Codex.Application.Dtos;

public sealed record CategoryDto(
    Guid Id,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc,
    string Name,
    int PostsCount)
    : EntityDto(Id, CreatedAtUtc, UpdatedAtUtc);