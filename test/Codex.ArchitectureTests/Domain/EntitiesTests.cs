using System.Reflection;
using Codex.ArchitectureTests.Bootstrapper;
using Codex.Domain.Entities.Base;
using NetArchTest.Rules;
using Shouldly;

namespace Codex.ArchitectureTests.Domain;

public sealed class EntitiesTests : BaseTest
{
    private static IEnumerable<Type> EntityTypes => Types
        .InAssembly(DomainAssembly)
        .That()
        .ResideInNamespaceContaining("Domain.Entities")
        .And()
        .Inherit(typeof(Entity))
        .GetTypes();

    [Fact]
    public void Entities_ShouldInheritFromEntityAbstractClass()
    {
        TestResult testResult = Types
            .InAssembly(DomainAssembly)
            .That()
            .ResideInNamespaceContaining("Domain.Entities")
            .And()
            .AreNotAbstract()
            .Should()
            .Inherit(typeof(Entity))
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue(
            $"The following types do not inherit from {nameof(Entity)}: {failingTypes}");
    }

    [Fact]
    public void Entities_ShouldBeSealed()
    {
        TestResult testResult = Types
            .InAssembly(DomainAssembly)
            .That()
            .ResideInNamespaceContaining("Domain.Entities")
            .And()
            .Inherit(typeof(Entity))
            .Should()
            .BeSealed()
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following entities are not sealed: {failingTypes}");
    }

    [Fact]
    public void Entities_PropertiesShouldHavePrivateSetters()
    {
        List<string> violations = [];

        foreach (Type type in EntityTypes)
        {
            IEnumerable<PropertyInfo> propertiesInfos = type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(propertyInfo => propertyInfo.CanWrite);

            foreach (PropertyInfo propertyInfo in propertiesInfos)
            {
                MethodInfo? setter = propertyInfo.GetSetMethod(true);

                if (setter is not null && !setter.IsPrivate)
                {
                    violations.Add($"{type.Name}.{propertyInfo.Name}");
                }
            }
        }

        string failingTypes = string.Join(", ", violations);
        violations.ShouldBeEmpty($"The following properties do not have private setters: {failingTypes}");
    }

    [Fact]
    public void Entities_ShouldHavePrivateConstructors()
    {
        List<string> violations = [];

        foreach (Type type in EntityTypes)
        {
            IEnumerable<ConstructorInfo> constructorInfos = type
                .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(constructorInfo => !constructorInfo.IsPrivate);

            if (constructorInfos.Any())
            {
                violations.Add(type.Name);
            }
        }

        string failingTypes = string.Join(", ", violations);
        violations.ShouldBeEmpty($"The following entities do not have only private constructors: {failingTypes}");
    }

    [Fact]
    public void Entities_ShouldNotHavePublicFields()
    {
        List<string> violations = [];

        foreach (Type type in EntityTypes)
        {
            IEnumerable<FieldInfo> fieldInfos = type
                .GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                violations.Add($"{type.Name}.{fieldInfo.Name}");
            }
        }

        string failingTypes = string.Join(", ", violations);
        violations.ShouldBeEmpty($"The following entities have public fields: {failingTypes}");
    }

    [Fact]
    public void Entities_ShouldResideInCorrectNamespace()
    {
        TestResult testResult = Types
            .InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(Entity))
            .And()
            .AreNotAbstract()
            .Should()
            .ResideInNamespaceContaining("Domain.Entities")
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue(
            $"The following entities do not reside in the correct namespace: {failingTypes}");
    }
}