using Codex.Application.Mediator;
using Codex.ArchitectureTests.Bootstrapper;
using NetArchTest.Rules;
using Shouldly;

namespace Codex.ArchitectureTests.Application;

public sealed class CommandsTests : BaseTest
{
    [Fact]
    public void Commands_CommandShouldBeSealed()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("Command")
            .And()
            .AreNotInterfaces()
            .Should()
            .BeSealed()
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following commands are not sealed: {failingTypes}");
    }

    [Fact]
    public void Commands_CommandShouldBePublic()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("Command")
            .And()
            .AreNotInterfaces()
            .Should()
            .BePublic()
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following commands are not public: {failingTypes}");
    }

    [Fact]
    public void Commands_CommandShouldImplementICommand()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("Command")
            .And()
            .AreNotInterfaces()
            .Should()
            .ImplementInterface(typeof(ICommand<>))
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following commands do not implement ICommand: {failingTypes}");
    }

    [Fact]
    public void Commands_CommandShouldResideInCommandsNamespace()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("Command")
            .And()
            .AreNotInterfaces()
            .Should()
            .ResideInNamespaceContaining("Application.Commands")
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue(
            $"The following commands do not reside in the Commands namespace: {failingTypes}");
    }

    [Fact]
    public void Commands_CommandHandlerShouldBeSealed()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("CommandHandler")
            .Should()
            .BeSealed()
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following command handlers are not sealed: {failingTypes}");
    }

    [Fact]
    public void Commands_CommandHandlerShouldBeInternal()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("CommandHandler")
            .Should()
            .NotBePublic()
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue($"The following command handlers are not internal: {failingTypes}");
    }

    [Fact]
    public void Commands_CommandHandlerShouldImplementICommandHandler()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("CommandHandler")
            .Should()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue(
            $"The following command handlers do not implement ICommandHandler: {failingTypes}");
    }

    [Fact]
    public void Commands_CommandHandlerShouldResideInCommandsNamespace()
    {
        TestResult testResult = Types
            .InAssembly(ApplicationAssembly)
            .That()
            .HaveNameEndingWith("CommandHandler")
            .Should()
            .ResideInNamespaceContaining("Application.Commands")
            .GetResult();

        string failingTypes = string.Join(", ", testResult.FailingTypeNames ?? []);
        testResult.IsSuccessful.ShouldBeTrue(
            $"The following command handlers do not reside in the Commands namespace: {failingTypes}");
    }
}