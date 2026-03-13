using Codex.Application.Dtos.Base;
using Codex.Domain.Enumerations;

namespace Codex.Application.Dtos;

public sealed record PostDto(
    Guid Id,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? UpdatedAtUtc,
    string Title,
    string Content,
    PostStatus Status,
    DateTimeOffset? PublishedAtUtc,
    AuthorDto Author,
    CategoryDto Category)
    : EntityDto(Id, CreatedAtUtc, UpdatedAtUtc);