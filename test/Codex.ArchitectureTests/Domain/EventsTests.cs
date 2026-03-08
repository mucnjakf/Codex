using Codex.ArchitectureTests.Bootstrapper;
using Codex.Domain.Events;
using NetArchTest.Rules;
using Shouldly;

namespace Codex.ArchitectureTests.Domain;

public sealed class EventsTests : BaseTest
{
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