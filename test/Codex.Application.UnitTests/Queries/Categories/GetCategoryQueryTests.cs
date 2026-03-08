using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Queries.Categories;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Queries.Categories;

public sealed class GetCategoryQueryTests
{
    private readonly GetCategoryQueryHandler _queryHandler;

    private readonly ICategoryRepository _categoryRepositoryMock;

    public GetCategoryQueryTests()
    {
        _categoryRepositoryMock = Substitute.For<ICategoryRepository>();

        _queryHandler = new GetCategoryQueryHandler(_categoryRepositoryMock);
    }

    [Fact]
    public async Task Handle_ShouldReturnCategory_WhenCategoryExists()
    {
        Category category = CategoryData.Category;

        _categoryRepositoryMock
            .GetAsync(category.Id, Arg.Any<CancellationToken>())
            .Returns(category);

        GetCategoryQuery query = new(category.Id);

        Result<CategoryDto> result = await _queryHandler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        result.Value.ShouldNotBeNull();
        result.Value.Id.ShouldBe(category.Id);
        result.Value.CreatedAtUtc.ShouldBe(category.CreatedAtUtc);
        result.Value.UpdatedAtUtc.ShouldBe(category.UpdatedAtUtc);
        result.Value.Name.ShouldBe(category.Name);
        result.Value.PostsCount.ShouldBe(category.Posts.Count);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenCategoryDoesNotExist()
    {
        Guid categoryId = CategoryData.Id;

        _categoryRepositoryMock
            .GetAsync(categoryId, Arg.Any<CancellationToken>())
            .Returns((Category)null!);

        GetCategoryQuery query = new(categoryId);

        Result<CategoryDto> result = await _queryHandler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(CategoryErrors.NotFound);
    }
}