using Codex.ArchitectureTests.Bootstrapper;
using NetArchTest.Rules;
using Shouldly;

namespace Codex.ArchitectureTests.Domain;

public sealed class EnumerationsTests : BaseTest
{
    private static IEnumerable<Type> EnumerationTypes => Types
        .InAssembly(DomainAssembly)
        .That()
        .ResideInNamespaceContaining("Domain.Enumerations")
        .GetTypes()
        .Where(type => type.IsEnum);

    [Fact]
    public void Enumerations_ShouldResideInCorrectNamespace()
    {
        List<Type> violations = Types
            .InAssembly(DomainAssembly)
            .GetTypes()
            .Where(type => type.IsEnum && type.Namespace != null && !type.Namespace.Contains("Domain.Enumerations"))
            .ToList();

        string failingTypes = string.Join(", ", violations.Select(type => type.Name));
        violations.ShouldBeEmpty($"The following enumerations do not reside in the correct namespace: {failingTypes}");
    }

    [Fact]
    public void Enumerations_ShouldBePublic()
    {
        List<Type> violations = EnumerationTypes
            .Where(type => !type.IsPublic)
            .ToList();

        string failingTypes = string.Join(", ", violations.Select(type => type.Name));
        violations.ShouldBeEmpty($"The following enumerations are not public: {failingTypes}");
    }
}