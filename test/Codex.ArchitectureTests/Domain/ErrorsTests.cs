using System.Reflection;
using Codex.ArchitectureTests.Bootstrapper;
using Codex.Domain.Outcomes;
using NetArchTest.Rules;
using Shouldly;

namespace Codex.ArchitectureTests.Domain;

public sealed class ErrorsTests : BaseTest
{
    [Fact]
    public void Errors_ShouldResideInErrorsNamespace()
    {
        TestResult testResult = Types
            .InAssembly(DomainAssembly)
            .That()
            .ResideInNamespace("Domain.Errors")
            .Should()
            .ResideInNamespaceContaining("Errors")
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue(
            $"The following error types do not reside in the correct namespace: {failingTypes}");
    }

    [Fact]
    public void Errors_ShouldBeStaticClasses()
    {
        List<Type> violations = Types
            .InAssembly(DomainAssembly)
            .That()
            .ResideInNamespace("Domain.Errors")
            .GetTypes()
            .Where(type => !(type.IsAbstract && type.IsSealed))
            .ToList();

        string failingTypes = string.Join(", ", violations.Select(type => type.Name));
        violations.ShouldBeEmpty($"The following error types are not static classes: {failingTypes}");
    }

    [Fact]
    public void Errors_ShouldHaveNameEndingWithErrors()
    {
        TestResult testResult = Types
            .InAssembly(DomainAssembly)
            .That()
            .ResideInNamespace("Domain.Errors")
            .Should()
            .HaveNameEndingWith("Errors")
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following error types do not end with 'Errors': {failingTypes}");
    }

    [Fact]
    public void Errors_AllMembersShouldBeStatic()
    {
        List<string> violations = Types
            .InAssembly(DomainAssembly)
            .That()
            .ResideInNamespace("Domain.Errors")
            .GetTypes()
            .SelectMany(type => type
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(fieldInfo => !fieldInfo.Name.StartsWith("<"))
                .Select(fieldInfo => $"{type.Name}.{fieldInfo.Name}"))
            .ToList();

        string failingMembers = string.Join(", ", violations);
        violations.ShouldBeEmpty($"The following members are not static: {failingMembers}");
    }

    [Fact]
    public void Errors_AllFieldsShouldBeOfTypeError()
    {
        List<string> violations = Types
            .InAssembly(DomainAssembly)
            .That()
            .ResideInNamespace("Domain.Errors")
            .GetTypes()
            .SelectMany(type => type
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
                .Where(fieldInfo => !fieldInfo.Name.StartsWith("<") && fieldInfo.FieldType != typeof(Error))
                .Select(fieldInfo => $"{type.Name}.{fieldInfo.Name}"))
            .ToList();

        string failingMembers = string.Join(", ", violations);
        violations.ShouldBeEmpty($"The following fields are not of type Error: {failingMembers}");
    }
}