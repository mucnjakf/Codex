using Codex.Application.Dtos.Base;
using Codex.ArchitectureTests.Bootstrapper;
using NetArchTest.Rules;
using Shouldly;

namespace Codex.ArchitectureTests.Application;

public sealed class DtosTests : BaseTest
{
    [Fact]
    public void Dtos_EntityDtoShouldBeAbstract()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveName(nameof(EntityDto))
            .Should()
            .BeAbstract()
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following DTOs are not abstract: {failingTypes}");
    }

    [Fact]
    public void Dtos_ShouldInheritFromEntityDto()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("Dto")
            .And()
            .AreNotAbstract()
            .Should()
            .Inherit(typeof(EntityDto))
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following DTOs do not inherit from EntityDto: {failingTypes}");
    }

    [Fact]
    public void Dtos_ShouldBeSealed()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("Dto")
            .And()
            .AreNotAbstract()
            .Should()
            .BeSealed()
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following DTOs are not sealed: {failingTypes}");
    }

    [Fact]
    public void Dtos_ShouldBePublic()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("Dto")
            .Should()
            .BePublic()
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following DTOs are not public: {failingTypes}");
    }

    [Fact]
    public void Dtos_ShouldResideInCorrectNamespace()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("Dto")
            .And()
            .AreNotAbstract()
            .Should()
            .ResideInNamespaceContaining("Application.Dtos")
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following DTOs do not reside in the Dtos namespace: {failingTypes}");
    }

    [Fact]
    public void Dtos_MappersShouldBeInternalAndStatic()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("Mapper")
            .Should()
            .BeStatic()
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following mappers are not static: {failingTypes}");
    }

    [Fact]
    public void Dtos_MappersShouldResideInCorrectNamespace()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("Mapper")
            .Should()
            .ResideInNamespaceContaining("Application.Dtos.Mapper")
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following mappers do not reside in the Mapper namespace: {failingTypes}");
    }
}