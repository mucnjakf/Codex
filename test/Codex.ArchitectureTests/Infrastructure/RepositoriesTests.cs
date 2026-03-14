using Codex.Application.Data;
using Codex.ArchitectureTests.Bootstrapper;
using NetArchTest.Rules;
using Shouldly;

namespace Codex.ArchitectureTests.Infrastructure;

public sealed class RepositoriesTests : BaseTest
{
    [Fact]
    public void Repositories_ShouldBeSealed()
    {
        TestResult testResult = Types
            .InAssembly(InfrastructureAssembly)
            .That()
            .HaveNameEndingWith("Repository")
            .Should()
            .BeSealed()
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following repositories are not sealed: {failingTypes}");
    }

    [Fact]
    public void Repositories_ShouldBeInternal()
    {
        TestResult testResult = Types
            .InAssembly(InfrastructureAssembly)
            .That()
            .HaveNameEndingWith("Repository")
            .Should()
            .NotBePublic()
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following repositories are not internal: {failingTypes}");
    }

    [Fact]
    public void Repositories_ShouldImplementRepositoryInterface()
    {
        TestResult testResult = Types
            .InAssembly(InfrastructureAssembly)
            .That()
            .HaveNameEndingWith("Repository")
            .And()
            .AreNotInterfaces()
            .Should()
            .ImplementInterface(typeof(IBaseRepository))
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue(
            $"The following repositories do not implement a repository interface: {failingTypes}");
    }

    [Fact]
    public void Repositories_ShouldResideInCorrectNamespace()
    {
        TestResult testResult = Types
            .InAssembly(InfrastructureAssembly)
            .That()
            .HaveNameEndingWith("Repository")
            .And()
            .AreNotInterfaces()
            .Should()
            .ResideInNamespaceContaining("Infrastructure.EfCore.Repositories")
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue(
            $"The following repositories do not reside in the correct namespace: {failingTypes}");
    }
}