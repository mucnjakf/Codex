using Codex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codex.Infrastructure.EfCore.EntityTypeConfiguration;

internal sealed class CommentEntityTypeConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");

        builder.HasKey(comment => comment.Id);

        builder
            .Property(comment => comment.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder
            .Property(comment => comment.CreatedAtUtc)
            .IsRequired();

        builder
            .Property(comment => comment.UpdatedAtUtc)
            .IsRequired(false);

        builder
            .Property(comment => comment.Content)
            .HasMaxLength(250)
            .IsRequired();

        builder
            .HasOne(comment => comment.Post)
            .WithMany(post => post.Comments)
            .HasForeignKey(comment => comment.PostId)
            .IsRequired();

        builder
            .HasOne(comment => comment.Reader)
            .WithMany(reader => reader.Comments)
            .HasForeignKey(comment => comment.ReaderId)
            .IsRequired();
    }
}