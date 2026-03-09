using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Mappers;
using Codex.Application.Dtos.Pagination;
using Codex.Application.Mediator;
using Codex.Domain.Entities;
using Codex.Domain.Outcomes;

namespace Codex.Application.Queries.Categories;

public sealed record GetCategoriesQuery(int PageNumber, int PageSize) : IQuery<PaginationDto<CategoryDto>>;

internal sealed class GetCategoriesQueryHandler(
    ICategoryRepository categoryRepository)
    : IQueryHandler<GetCategoriesQuery, PaginationDto<CategoryDto>>
{
    public async Task<Result<PaginationDto<CategoryDto>>> Handle(
        GetCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        PaginationDto<Category> paginatedCategories = await categoryRepository
            .GetPaginatedAsync(query.PageNumber, query.PageSize, cancellationToken);

        return new PaginationDto<CategoryDto>
        {
            Items = paginatedCategories.Items.Select(category => category.ToCategoryDto()).ToList(),
            PageNumber = paginatedCategories.PageNumber,
            PageSize = paginatedCategories.PageSize,
            TotalCount = paginatedCategories.TotalCount
        };
    }
}