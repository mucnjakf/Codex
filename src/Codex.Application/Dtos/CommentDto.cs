using Codex.Application.Dtos.Base;

namespace Codex.Application.Dtos;

public sealed record CommentDto(
    Guid Id,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc,
    string Content,
    PostDto Post,
    ReaderDto Reader)
    : EntityDto(Id, CreatedAtUtc, UpdatedAtUtc);