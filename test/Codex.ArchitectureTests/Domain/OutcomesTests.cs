using Codex.ArchitectureTests.Infrastructure;
using Codex.Domain.Outcomes;
using NetArchTest.Rules;
using Shouldly;

namespace Codex.ArchitectureTests.Domain;

public sealed class OutcomesTests : BaseTest
{
    [Fact]
    public void Outcomes_ShouldResideInOutcomesNamespace()
    {
        TestResult testResult = Types
            .InAssembly(DomainAssembly)
            .That()
            .HaveNameMatching("^(Result|Error)$")
            .Should()
            .ResideInNamespaceContaining("Domain.Outcomes")
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue(
            $"The following outcome types do not reside in the correct namespace: {failingTypes}");
    }

    [Fact]
    public void Outcomes_ShouldBePublic()
    {
        TestResult testResult = Types
            .InAssembly(DomainAssembly)
            .That()
            .ResideInNamespaceContaining("Domain.Outcomes")
            .Should()
            .BePublic()
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following outcome types are not public: {failingTypes}");
    }

    [Fact]
    public void Result_ShouldNotBeSealed()
    {
        Type resultType = typeof(Result);
        resultType.IsSealed.ShouldBeFalse("Result should not be sealed");
    }

    [Fact]
    public void ResultOfTValue_ShouldBeSealed()
    {
        Type resultType = typeof(Result<>);
        resultType.IsSealed.ShouldBeTrue("Result<TValue> should be sealed");
    }

    [Fact]
    public void Error_ShouldBeSealed()
    {
        Type errorType = typeof(Error);
        errorType.IsSealed.ShouldBeTrue("Error should be sealed.");
    }
}