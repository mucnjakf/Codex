using Codex.Application.Data;
using Codex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Codex.Infrastructure.EfCore;

public sealed class
    ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IUnitOfWork // TODO: public becasue applymig
{
    internal DbSet<Author> Authors { get; init; } = null!;

    internal DbSet<Category> Categories { get; init; } = null!;

    internal DbSet<Comment> Comments { get; init; } = null!;

    internal DbSet<Post> Posts { get; init; } = null!;

    internal DbSet<Reader> Readers { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InfrastructureModule).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}

public sealed class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=codex;Username=postgres;Password=postgres")
            .Options;

        return new ApplicationDbContext(options);
    }
}