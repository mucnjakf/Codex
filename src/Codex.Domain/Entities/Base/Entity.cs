using Codex.Domain.Events;

namespace Codex.Domain.Entities.Base;

public abstract class Entity(Guid id, DateTimeOffset createdAtUtc)
{
    public Guid Id { get; private set; } = id;

    public DateTimeOffset CreatedAtUtc { get; private set; } = createdAtUtc;

    public DateTimeOffset? UpdatedAtUtc { get; protected set; }

    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}