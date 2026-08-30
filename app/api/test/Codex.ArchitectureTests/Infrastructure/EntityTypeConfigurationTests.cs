using Codex.ArchitectureTests.Bootstrapper;
using Microsoft.EntityFrameworkCore;
using NetArchTest.Rules;
using Shouldly;

namespace Codex.ArchitectureTests.Infrastructure;

public sealed class EntityTypeConfigurationTests : BaseTest
{
    [Fact]
    public void EntityTypeConfiguration_ShouldBeSealed()
    {
        TestResult testResult = Types
            .InAssembly(InfrastructureAssembly)
            .That()
            .HaveNameEndingWith("EntityTypeConfiguration")
            .Should()
            .BeSealed()
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue(
            $"The following entity type configuration are not sealed: {failingTypes}");
    }

    [Fact]
    public void EntityTypeConfiguration_ShouldBeInternal()
    {
        TestResult testResult = Types
            .InAssembly(InfrastructureAssembly)
            .That()
            .HaveNameEndingWith("EntityTypeConfiguration")
            .Should()
            .NotBePublic()
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue(
            $"The following entity type configuration are not internal: {failingTypes}");
    }

    [Fact]
    public void EntityTypeConfiguration_ShouldImplementIEntityTypeConfiguration()
    {
        TestResult testResult = Types
            .InAssembly(InfrastructureAssembly)
            .That()
            .HaveNameEndingWith("EntityTypeConfiguration")
            .Should()
            .ImplementInterface(typeof(IEntityTypeConfiguration<>))
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue(
            $"The following entity type configuration do not implement IEntityTypeConfiguration<>: {failingTypes}");
    }

    [Fact]
    public void EntityTypeConfiguration_ShouldResideInCorrectNamespace()
    {
        TestResult testResult = Types
            .InAssembly(InfrastructureAssembly)
            .That()
            .HaveNameEndingWith("EntityTypeConfiguration")
            .Should()
            .ResideInNamespaceContaining("Infrastructure.EfCore.EntityTypeConfiguration")
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue(
            $"The following entity type configuration do not reside in the correct namespace: {failingTypes}");
    }
}