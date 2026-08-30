using Codex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codex.Infrastructure.EfCore.EntityTypeConfiguration;

internal sealed class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(category => category.Id);

        builder
            .Property(category => category.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder
            .Property(category => category.CreatedAtUtc)
            .IsRequired();

        builder
            .Property(category => category.UpdatedAtUtc)
            .IsRequired(false);

        builder
            .Property(category => category.Name)
            .HasMaxLength(30)
            .IsRequired();
    }
}