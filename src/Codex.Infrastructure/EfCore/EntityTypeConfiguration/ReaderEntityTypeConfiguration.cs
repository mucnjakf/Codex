using Codex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codex.Infrastructure.EfCore.EntityTypeConfiguration;

internal sealed class ReaderEntityTypeConfiguration : IEntityTypeConfiguration<Reader>
{
    public void Configure(EntityTypeBuilder<Reader> builder)
    {
        builder.ToTable("Readers");

        builder.HasKey(reader => reader.Id);

        builder
            .Property(reader => reader.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder
            .Property(reader => reader.CreatedAtUtc)
            .IsRequired();

        builder
            .Property(reader => reader.UpdatedAtUtc)
            .IsRequired(false);

        builder
            .Property(reader => reader.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(reader => reader.LastName)
            .HasMaxLength(50)
            .IsRequired();
    }
}