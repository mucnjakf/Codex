using Codex.ArchitectureTests.Bootstrapper;
using Codex.Application.Mediator;
using NetArchTest.Rules;
using Shouldly;

namespace Codex.ArchitectureTests.Application;

public sealed class QueriesTests : BaseTest
{
    [Fact]
    public void Queries_QueryShouldBeSealed()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("Query")
            .And()
            .AreNotInterfaces()
            .Should()
            .BeSealed()
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following queries are not sealed: {failingTypes}");
    }

    [Fact]
    public void Queries_QueryShouldBePublic()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("Query")
            .And()
            .AreNotInterfaces()
            .Should()
            .BePublic()
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following queries are not public: {failingTypes}");
    }

    [Fact]
    public void Queries_QueryShouldImplementIQuery()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("Query")
            .And()
            .AreNotInterfaces()
            .Should()
            .ImplementInterface(typeof(IQuery<>))
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following queries do not implement IQuery: {failingTypes}");
    }

    [Fact]
    public void Queries_QueryShouldResideInQueriesNamespace()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("Query")
            .And()
            .AreNotInterfaces()
            .Should()
            .ResideInNamespaceContaining("Application.Queries")
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue(
            $"The following queries do not reside in the Queries namespace: {failingTypes}");
    }

    [Fact]
    public void Queries_QueryHandlerShouldBeSealed()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("QueryHandler")
            .Should()
            .BeSealed()
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following query handlers are not sealed: {failingTypes}");
    }

    [Fact]
    public void Queries_QueryHandlerShouldBeInternal()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("QueryHandler")
            .Should()
            .NotBePublic()
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following query handlers are not internal: {failingTypes}");
    }

    [Fact]
    public void Queries_QueryHandlerShouldImplementIQueryHandler()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("QueryHandler")
            .Should()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue(
            $"The following query handlers do not implement IQueryHandler: {failingTypes}");
    }

    [Fact]
    public void Queries_QueryHandlerShouldResideInQueriesNamespace()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("QueryHandler")
            .Should()
            .ResideInNamespaceContaining("Application.Queries")
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue(
            $"The following query handlers do not reside in the Queries namespace: {failingTypes}");
    }
}