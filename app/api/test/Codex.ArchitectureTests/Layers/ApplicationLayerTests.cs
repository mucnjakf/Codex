using Codex.ArchitectureTests.Bootstrapper;
using NetArchTest.Rules;
using Shouldly;

namespace Codex.ArchitectureTests.Layers;

public sealed class ApplicationLayerTests : BaseTest
{
    [Fact]
    public void ApplicationLayer_ShouldNotHaveDependencyOnInfrastructureLayer()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .Should()
            .NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue(
            $"The following types have a dependency on the Infrastructure layer: {failingTypes}");
    }
}