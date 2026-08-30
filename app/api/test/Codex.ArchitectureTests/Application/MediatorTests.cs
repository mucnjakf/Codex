using Codex.ArchitectureTests.Bootstrapper;
using NetArchTest.Rules;
using Shouldly;

namespace Codex.ArchitectureTests.Application;

public sealed class MediatorTests : BaseTest
{
    [Fact]
    public void Outcomes_ShouldResideInCorrectNamespace()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("^(Command|CommandHandler|Query|QueryHandler)$")
            .And()
            .AreInterfaces()
            .Should()
            .ResideInNamespaceContaining("Application.Mediator")
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue(
            $"The following outcome types do not reside in the correct namespace: {failingTypes}");
    }

    [Fact]
    public void ICommandHandler_ShouldBeInternal()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("CommandHandler")
            .And()
            .AreInterfaces()
            .Should()
            .NotBePublic()
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue(
            $"The following command handler interfaces are not internal: {failingTypes}");
    }

    [Fact]
    public void IQueryHandler_ShouldBeInternal()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("QueryHandler")
            .And()
            .AreInterfaces()
            .Should()
            .NotBePublic()
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue(
            $"The following query handler interfaces are not internal: {failingTypes}");
    }
}