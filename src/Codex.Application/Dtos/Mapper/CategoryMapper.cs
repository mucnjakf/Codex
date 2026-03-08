using Codex.Domain.Entities;

namespace Codex.Application.Dtos.Mapper;

internal static class CategoryMapper
{
    extension(Category category)
    {
        internal CategoryDto ToCategoryDto() => new(
            category.Id,
            category.CreatedAtUtc,
            category.UpdatedAtUtc,
            category.Name,
            category.Posts.Count);
    }
}