using Codex.ArchitectureTests.Infrastructure;
using Codex.Domain.Events;
using NetArchTest.Rules;
using Shouldly;

namespace Codex.ArchitectureTests.Domain;

public sealed class EventsTests : BaseTest
{
    [Fact]
    public void Events_ShouldResideInEventsNamespace()
    {
        TestResult testResult = Types
            .InAssembly(DomainAssembly)
            .That()
            .HaveNameEndingWith("DomainEvent")
            .Should()
            .ResideInNamespaceContaining("Domain.Events")
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue(
            $"The following events do not reside in the correct namespace: {failingTypes}");
    }

    [Fact]
    public void Events_ShouldImplementIDomainEvent()
    {
        TestResult testResult = Types
            .InAssembly(DomainAssembly)
            .That()
            .HaveNameEndingWith("DomainEvent")
            .And()
            .AreNotInterfaces()
            .Should()
            .ImplementInterface(typeof(IDomainEvent))
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue(
            $"The following events do not implement {nameof(IDomainEvent)}: {failingTypes}");
    }

    [Fact]
    public void Events_ShouldBeSealed()
    {
        TestResult testResult = Types
            .InAssembly(DomainAssembly)
            .That()
            .HaveNameEndingWith("DomainEvent")
            .And()
            .AreNotInterfaces()
            .Should()
            .BeSealed()
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following events are not sealed: {failingTypes}");
    }

    [Fact]
    public void Events_ShouldBeImmutable()
    {
        List<Type> violations = Types
            .InAssembly(DomainAssembly)
            .That()
            .HaveNameEndingWith("DomainEvent")
            .GetTypes()
            .Where(type => type.GetProperties()
                .Any(propertyInfo => propertyInfo.SetMethod != null && propertyInfo.SetMethod.IsPublic))
            .ToList();

        string failingTypes = string.Join(", ", violations.Select(type => type.Name));
        violations.ShouldBeEmpty($"The following events have public setters: {failingTypes}");
    }

    [Fact]
    public void Events_ShouldHaveNameEndingWithDomainEvent()
    {
        List<Type> violations = Types
            .InAssembly(DomainAssembly)
            .GetTypes()
            .Where(type => typeof(IDomainEvent).IsAssignableFrom(type)
                           && type != typeof(IDomainEvent)
                           && !type.Name.EndsWith("DomainEvent", StringComparison.OrdinalIgnoreCase))
            .ToList();

        string failingTypes = string.Join(", ", violations.Select(type => type.Name));
        violations.ShouldBeEmpty($"The following events do not end with 'DomainEvent': {failingTypes}");
    }

    [Fact]
    public void Events_ShouldBePublic()
    {
        TestResult testResult = Types
            .InAssembly(DomainAssembly)
            .That()
            .HaveNameEndingWith("DomainEvent")
            .And()
            .AreNotInterfaces()
            .Should()
            .BePublic()
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following events are not public: {failingTypes}");
    }
}