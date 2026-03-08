namespace Codex.Application.Dtos.Base;

public abstract record EntityDto(Guid Id, DateTimeOffset CreatedAtUtc, DateTimeOffset? UpdatedAtUtc);