using Codex.Application.Data;
using Codex.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Codex.Infrastructure.EfCore;

internal sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IUnitOfWork
{
    internal DbSet<Author> Authors { get; } = null!;

    internal DbSet<Category> Categories { get; } = null!;

    internal DbSet<Comment> Comments { get; } = null!;

    internal DbSet<Post> Posts { get; } = null!;

    internal DbSet<Reader> Readers { get; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InfrastructureModule).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}