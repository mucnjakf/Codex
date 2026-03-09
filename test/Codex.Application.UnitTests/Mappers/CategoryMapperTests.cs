using Codex.Application.Dtos.Mappers;
using Codex.Domain.Entities;
using Codex.Tests.Data;
using Shouldly;

namespace Codex.Application.UnitTests.Mappers;

public sealed class CategoryMapperTests
{
    [Fact]
    public void ToCategoryDto_ShouldReturnCategoryDto()
    {
        Category category = CategoryData.Category;

        var categoryDto = category.ToCategoryDto();

        categoryDto.ShouldNotBeNull();
        categoryDto.Id.ShouldBe(category.Id);
        categoryDto.CreatedAtUtc.ShouldBe(category.CreatedAtUtc);
        categoryDto.UpdatedAtUtc.ShouldBe(category.UpdatedAtUtc);
        categoryDto.Name.ShouldBe(category.Name);
        categoryDto.PostsCount.ShouldBe(category.Posts.Count);
    }
}