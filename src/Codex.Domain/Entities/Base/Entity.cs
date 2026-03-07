using Codex.Domain.Events;

namespace Codex.Domain.Entities.Base;

public abstract class Entity(Guid id, DateTimeOffset createdAtUtc)
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public Guid Id { get; private set; } = id;

    public DateTimeOffset CreatedAtUtc { get; private set; } = createdAtUtc;

    public DateTimeOffset? UpdatedAtUtc { get; private set; }

    protected void UpdateUpdatedAtUtc(DateTimeOffset updatedAtUtc) => UpdatedAtUtc = updatedAtUtc;

    public IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}