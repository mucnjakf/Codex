using Codex.ArchitectureTests.Infrastructure;
using NetArchTest.Rules;
using Shouldly;

namespace Codex.ArchitectureTests.Domain;

public sealed class DataTests : BaseTest
{
    [Fact]
    public void Data_ShouldResideInDataNamespace()
    {
        TestResult testResult = Types
            .InAssembly(DomainAssembly)
            .That()
            .ResideInNamespace("Domain.Data")
            .Should()
            .ResideInNamespaceContaining("Data")
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue(
            $"The following data types do not reside in the correct namespace: {failingTypes}");
    }

    [Fact]
    public void Data_ShouldBePublic()
    {
        TestResult testResult = Types
            .InAssembly(DomainAssembly)
            .That()
            .ResideInNamespace("Domain.Data")
            .Should()
            .BePublic()
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following data types are not public: {failingTypes}");
    }

    [Fact]
    public void Data_ShouldOnlyContainInterfaces()
    {
        List<Type> violations = Types
            .InAssembly(DomainAssembly)
            .That()
            .ResideInNamespace("Domain.Data")
            .GetTypes()
            .Where(type => !type.IsInterface)
            .ToList();

        string failingTypes = string.Join(", ", violations.Select(type => type.Name));
        violations.ShouldBeEmpty($"The following data types are not interfaces: {failingTypes}");
    }
}