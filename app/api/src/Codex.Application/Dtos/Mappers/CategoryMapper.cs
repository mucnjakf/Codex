using Codex.Domain.Entities;

namespace Codex.Application.Dtos.Mappers;

internal static class CategoryMapper
{
    extension(Category category)
    {
        internal CategoryDto ToCategoryDto() => new(
            category.Id,
            category.CreatedAtUtc,
            category.UpdatedAtUtc,
            category.Name);
    }
}