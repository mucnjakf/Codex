using Codex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Codex.Infrastructure.EfCore.EntityTypeConfiguration;

internal sealed class PostEntityTypeConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("Posts");

        builder.HasKey(post => post.Id);

        builder
            .Property(post => post.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder
            .Property(post => post.CreatedAtUtc)
            .IsRequired();

        builder
            .Property(post => post.UpdatedAtUtc)
            .IsRequired(false);

        builder
            .Property(post => post.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(post => post.Content)
            .HasMaxLength(1000)
            .IsRequired();

        builder
            .Property(post => post.Status)
            .IsRequired();

        builder
            .Property(post => post.PublishedAtUtc)
            .IsRequired(false);

        builder
            .HasOne(post => post.Author)
            .WithMany(author => author.Posts)
            .HasForeignKey(post => post.AuthorId)
            .IsRequired();

        builder
            .HasOne(post => post.Category)
            .WithMany(category => category.Posts)
            .HasForeignKey(post => post.CategoryId)
            .IsRequired();
    }
}