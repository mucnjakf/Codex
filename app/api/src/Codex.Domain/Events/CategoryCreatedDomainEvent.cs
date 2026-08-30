namespace Codex.Domain.Events;

public sealed record CategoryCreatedDomainEvent(Guid CategoryId) : IDomainEvent;