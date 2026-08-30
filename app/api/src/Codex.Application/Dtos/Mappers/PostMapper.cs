using Codex.Domain.Entities;

namespace Codex.Application.Dtos.Mappers;

internal static class PostMapper
{
    extension(Post post)
    {
        internal PostDto ToPostDto() => new(
            post.Id,
            post.CreatedAtUtc,
            post.UpdatedAtUtc,
            post.Title,
            post.Content,
            post.Status,
            post.PublishedAtUtc,
            post.Author.ToAuthorDto(),
            post.Category.ToCategoryDto());
    }
}