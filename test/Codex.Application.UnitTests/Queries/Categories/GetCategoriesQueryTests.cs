using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Pagination;
using Codex.Application.Queries.Categories;
using Codex.Domain.Entities;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Queries.Categories;

public sealed class GetCategoriesQueryTests
{
    private readonly GetCategoriesQueryHandler _queryHandler;

    private readonly ICategoryRepository _categoryRepositoryMock;

    public GetCategoriesQueryTests()
    {
        _categoryRepositoryMock = Substitute.For<ICategoryRepository>();

        _queryHandler = new GetCategoriesQueryHandler(_categoryRepositoryMock);
    }

    [Fact]
    public async Task Handle_ShouldReturnCategories_WhenCategoriesExist()
    {
        IReadOnlyList<Category> categories = [CategoryData.Category];
        const int pageNumber = 1;
        const int pageSize = 1;

        var paginatedCategories = new PaginationDto<Category>()
        {
            Items = categories,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = categories.Count
        };

        _categoryRepositoryMock
            .GetPaginatedAsync(
                paginatedCategories.PageNumber,
                paginatedCategories.PageSize,
                Arg.Any<CancellationToken>())
            .Returns(paginatedCategories);

        GetCategoriesQuery query = new(paginatedCategories.PageNumber, paginatedCategories.PageSize);

        Result<PaginationDto<CategoryDto>> result = await _queryHandler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        result.Value.ShouldNotBeNull();
        result.Value.Items.Count.ShouldBe(paginatedCategories.Items.Count);
        result.Value.Items[0].Id.ShouldBe(paginatedCategories.Items[0].Id);
        result.Value.PageNumber.ShouldBe(paginatedCategories.PageNumber);
        result.Value.PageSize.ShouldBe(paginatedCategories.PageSize);
        result.Value.TotalCount.ShouldBe(paginatedCategories.TotalCount);
        result.Value.TotalPages.ShouldBe(paginatedCategories.TotalPages);
        result.Value.HasPreviousPage.ShouldBe(paginatedCategories.HasPreviousPage);
        result.Value.HasNextPage.ShouldBe(paginatedCategories.HasNextPage);
    }
}