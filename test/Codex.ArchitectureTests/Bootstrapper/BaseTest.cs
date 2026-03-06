using System.Reflection;
using Codex.Application;
using Codex.Domain;
using Codex.Infrastructure;

namespace Codex.ArchitectureTests.Bootstrapper;

public abstract class BaseTest
{
    protected static readonly Assembly DomainAssembly = typeof(DomainModule).Assembly;

    protected static readonly Assembly ApplicationAssembly = typeof(ApplicationModule).Assembly;

    protected static readonly Assembly InfrastructureAssembly = typeof(InfrastructureModule).Assembly;
}