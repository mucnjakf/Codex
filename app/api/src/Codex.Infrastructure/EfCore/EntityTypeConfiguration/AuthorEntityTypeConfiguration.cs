using Codex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codex.Infrastructure.EfCore.EntityTypeConfiguration;

internal sealed class AuthorEntityTypeConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.ToTable("Authors");

        builder.HasKey(author => author.Id);

        builder
            .Property(author => author.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder
            .Property(comment => comment.CreatedAtUtc)
            .IsRequired();

        builder
            .Property(comment => comment.UpdatedAtUtc)
            .IsRequired(false);

        builder
            .Property(author => author.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(author => author.LastName)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(author => author.Biography)
            .HasMaxLength(100)
            .IsRequired();
    }
}