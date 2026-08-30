using Codex.ArchitectureTests.Bootstrapper;
using NetArchTest.Rules;
using Shouldly;

namespace Codex.ArchitectureTests.Application;

public sealed class DataTests : BaseTest
{
    [Fact]
    public void Data_ShouldBeInterfaces()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .ResideInNamespaceContaining("Application.Data")
            .Should()
            .BeInterfaces()
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following types are not interfaces: {failingTypes}");
    }

    [Fact]
    public void Data_ShouldBePublic()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .ResideInNamespaceContaining("Application.Data")
            .Should()
            .BePublic()
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following types are not public: {failingTypes}");
    }
}