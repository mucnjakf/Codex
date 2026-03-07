using Codex.Domain.Entities.Base;
using Codex.Domain.Events;

namespace Codex.Domain.UnitTests.Bootstrapper;

public abstract class BaseTest
{
    internal static T AssertDomainEventWasPublished<T>(Entity entity) where T : IDomainEvent
    {
        T? domainEvent = entity.GetDomainEvents().OfType<T>().SingleOrDefault();

        return domainEvent ?? throw new Exception($"{typeof(T).Name} was not published");
    }
}